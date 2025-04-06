using System;

namespace Shared.Messaging.Events;

public class AppointmentCreatedIntegrationEvent : IntegrationEvent, IIntegrationEvent
{
    public Guid AppointmentId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid LawyerId { get; set; }
    public DateTime ScheduledAt { get; set; }
}
