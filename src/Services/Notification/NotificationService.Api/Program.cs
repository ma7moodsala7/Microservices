using Microsoft.Extensions.Logging;
using NotificationService.Api.Consumers;
using Shared.Messaging;
using Shared.Settings;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Shared.Logging;

var builder = WebApplication.CreateBuilder(args);

// Load shared configuration
builder.Configuration.AddSharedSettings();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add MassTransit with consumer
var logger = LoggerFactory.Create(config => 
{
    config.AddConsole();
    config.SetMinimumLevel(LogLevel.Debug);
}).CreateLogger<Program>();

logger.LogInformation("Configuring MassTransit with consumer: {ConsumerType}", typeof(AppointmentCreatedConsumer).Name);
builder.Services.AddSharedMassTransit(typeof(AppointmentCreatedConsumer));
logger.LogInformation("MassTransit configured successfully");

// Add OpenTelemetry
builder.Services.AddOpenTelemetrySupport(builder.Configuration, "NotificationService");

// Add health checks
builder.Services.AddHealthChecks()
    .AddRabbitMQ($"amqp://guest:guest@{builder.Configuration["RabbitMQ:Host"]}:5672", name: "rabbitmq", tags: new[] { "ready" });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "NotificationService is running!");

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

app.Run();
