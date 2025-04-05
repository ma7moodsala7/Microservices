namespace Shared.Messaging.Events;

public class UserPingedIntegrationEvent : IntegrationEvent, IIntegrationEvent
{
    public string UserId { get; set; } = default!;
    public string Message { get; set; } = default!;
}
