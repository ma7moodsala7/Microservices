using Microsoft.Extensions.Logging;
using NotificationService.Api.Consumers;
using Shared.Messaging;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "NotificationService is running!");

app.Run();
