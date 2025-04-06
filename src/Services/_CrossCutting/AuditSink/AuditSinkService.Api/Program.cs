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

        cfg.ReceiveEndpoint(rabbitConfig["QueueName"], e =>
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

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AuditDbContext>();
    await context.Database.MigrateAsync();
}

app.Run();
