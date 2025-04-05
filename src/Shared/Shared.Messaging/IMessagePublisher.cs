namespace Shared.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
        where T : class, IIntegrationEvent;
}
