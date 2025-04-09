using Microsoft.Extensions.Logging;
using NotificationService.Api.Consumers;
using NotificationService.Application.Abstractions;
using NotificationService.Application.Dispatching;
using NotificationService.Application.Models;
using NotificationService.Application.Senders;
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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Notification API", Version = "v1" });
});

// Add notification infrastructure
// Register HttpClient for webhook sender
builder.Services.AddHttpClient<WebhookNotificationSender>();

// Register concrete sender implementations
builder.Services.AddSingleton<ConsoleNotificationSender>();
builder.Services.AddSingleton<SmsNotificationSender>();
builder.Services.AddSingleton<EmailNotificationSender>();
builder.Services.AddSingleton<WebhookNotificationSender>();

// Register sender interfaces
builder.Services.AddSingleton<INotificationSender>(sp => sp.GetRequiredService<ConsoleNotificationSender>());
builder.Services.AddSingleton<INotificationSender>(sp => sp.GetRequiredService<SmsNotificationSender>());
builder.Services.AddSingleton<INotificationSender>(sp => sp.GetRequiredService<EmailNotificationSender>());
builder.Services.AddSingleton<INotificationSender>(sp => sp.GetRequiredService<WebhookNotificationSender>());
builder.Services.AddSingleton<ISmsNotificationSender>(sp => sp.GetRequiredService<SmsNotificationSender>());
builder.Services.AddSingleton<IEmailNotificationSender>(sp => sp.GetRequiredService<EmailNotificationSender>());
builder.Services.AddSingleton<IWebhookNotificationSender>(sp => sp.GetRequiredService<WebhookNotificationSender>());

// Register dispatcher
builder.Services.AddSingleton<NotificationDispatcher>();

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
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification API V1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at the root
    });
}

app.MapGet("/", () => "NotificationService is running!");

// Add test notification endpoint
app.MapPost("/send-test-notification", async (
    NotificationMessage message,
    NotificationDispatcher dispatcher,
    ILogger<Program> logger,
    CancellationToken cancellationToken) =>
{
    try
    {
        await dispatcher.DispatchAsync(message, cancellationToken);
        return Results.Ok("Notification sent successfully");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to send test notification");
        return Results.Problem("Failed to send notification");
    }
});

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
