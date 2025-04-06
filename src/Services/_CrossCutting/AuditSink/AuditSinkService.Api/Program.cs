using AuditSinkService.Application.Features.CreateAuditLog;
using Microsoft.EntityFrameworkCore;
using MediatR;
using AuditSinkService.Persistence;
using MassTransit;
using AuditSinkService.Application.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<AuditDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add MediatR
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(CreateAuditLogCommand).Assembly));

// Add MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<AuditMessageConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitConfig = builder.Configuration.GetSection("RabbitMQ");
        
        cfg.Host(rabbitConfig["Host"], h =>
        {
            h.Username(rabbitConfig["Username"]);
            h.Password(rabbitConfig["Password"]);
        });

        var queueName = rabbitConfig["QueueName"] ?? "audit-queue";
        cfg.ReceiveEndpoint(queueName, e =>
        {
            e.ConfigureConsumer<AuditMessageConsumer>(context);
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Audit log endpoint
app.MapPost("/api/audit", async (CreateAuditLogCommand command, IMediator mediator) =>
{
    var auditLogId = await mediator.Send(command);
    return Results.Created($"/api/audit/{auditLogId}", auditLogId);
})
.WithName("CreateAuditLog")
.WithOpenApi();

// Initialize database and apply migrations
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<AuditDbContext>();
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        }

        var connBuilder = new Npgsql.NpgsqlConnectionStringBuilder(connectionString);
        var dbName = connBuilder.Database;
        connBuilder.Database = "postgres";

        // Create database if it doesn't exist
        using (var masterConnection = new Npgsql.NpgsqlConnection(connBuilder.ConnectionString))
        {
            await masterConnection.OpenAsync();
            using var cmd = masterConnection.CreateCommand();

            // Check if database exists
            cmd.CommandText = $"SELECT 1 FROM pg_database WHERE datname = '{dbName}'";
            var exists = await cmd.ExecuteScalarAsync() != null;

            if (!exists)
            {
                // Create database with template0 and C locale
                cmd.CommandText = $"CREATE DATABASE {dbName} WITH TEMPLATE template0 LC_COLLATE 'C' LC_CTYPE 'C';";
                await cmd.ExecuteNonQueryAsync();
            }

            await masterConnection.CloseAsync();
        }

        // Ensure database is created and migrations are applied
        await context.Database.EnsureCreatedAsync();
        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            Console.WriteLine("Applying pending migrations...");
            await context.Database.MigrateAsync();
            Console.WriteLine("Migrations applied successfully.");
        }
        else
        {
            Console.WriteLine("No pending migrations found.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during database initialization: {ex.Message}");
        throw;
    }
}

app.Run();
