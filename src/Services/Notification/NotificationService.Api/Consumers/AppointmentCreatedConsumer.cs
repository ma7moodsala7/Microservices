using MassTransit;
using Shared.Messaging.Events;

namespace NotificationService.Api.Consumers;

public class AppointmentCreatedConsumer : IConsumer<AppointmentCreatedIntegrationEvent>
{
    private readonly ILogger<AppointmentCreatedConsumer> _logger;

    public AppointmentCreatedConsumer(ILogger<AppointmentCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<AppointmentCreatedIntegrationEvent> context)
    {
        _logger.LogInformation("Starting to consume AppointmentCreatedIntegrationEvent");
        
        var message = context.Message;
        _logger.LogInformation(
            "[NOTIFICATION RECEIVED] Appointment {AppointmentId} created for Customer {CustomerId} with Lawyer {LawyerId} at {ScheduledAt}", 
            message.AppointmentId,
            message.CustomerId,
            message.LawyerId, 
            message.ScheduledAt);

        // TODO: Send real email/notification later
        _logger.LogInformation("Successfully processed notification for appointment {AppointmentId}", message.AppointmentId);
        
        return Task.CompletedTask;
    }
}
