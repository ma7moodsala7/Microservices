using IdentityService.Domain.Entities;
using IdentityService.Persistence;
using IdentityService.Persistence.Context;
using Shared.Settings;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using IdentityService.Application.Features.Auth.Commands;
using Shared.Logging;
using Shared.Messaging;
using Shared.Messaging.Events;
using IdentityService.API.Consumers;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Load shared configuration
builder.Configuration.AddSharedSettings();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));

// Add MassTransit with RabbitMQ
builder.Services.AddSharedMassTransit(typeof(UserPingedConsumer));

// Configure Serilog
builder.Host.UseSharedSerilog();

// Add OpenTelemetry
builder.Services.AddOpenTelemetrySupport(builder.Configuration, "IdentityService");

// Add health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<IdentityDbContext>("db", tags: new[] { "ready" })
    .AddRabbitMQ($"amqp://guest:guest@{builder.Configuration["RabbitMQ:Host"]}:5672", name: "rabbitmq", tags: new[] { "ready" });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseRouting();

// Add authentication/authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapControllers();

// Add health endpoints
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false, // just confirms the app is running
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapDefaultControllerRoute();

app.MapPost("/identity/test/publish", async (IMessagePublisher publisher) =>
{
    var evt = new UserPingedIntegrationEvent
    {
        UserId = "test-user",
        Message = "Hello from IdentityService!"
    };

    await publisher.PublishAsync(evt);

    return Results.Ok("Published UserPingedIntegrationEvent.");
});



// Apply migrations and seed test user
using (var scope = app.Services.CreateScope())
{
    // Apply migrations
    var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    await dbContext.Database.MigrateAsync();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    var existingUser = await userManager.FindByEmailAsync("test@shwra.com");
    if (existingUser == null)
    {
        var user = new User
        {
            UserName = "test@shwra.com",
            Email = "test@shwra.com",
            PhoneNumber = "+966501234567",
            EmailConfirmed = true, // For testing purposes
            FirstName = "Test",
            LastName = "User"
        };

        var result = await userManager.CreateAsync(user, "Shwra123$");
        if (!result.Succeeded)
        {
            throw new Exception($"Failed to create test user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
}



app.Run();