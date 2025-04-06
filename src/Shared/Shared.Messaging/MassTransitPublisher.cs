using MassTransit;
using Microsoft.Extensions.Logging;

namespace Shared.Messaging;

public class MassTransitPublisher : IMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<MassTransitPublisher> _logger;

    public MassTransitPublisher(
        IPublishEndpoint publishEndpoint,
        ILogger<MassTransitPublisher> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
        where T : class, IIntegrationEvent
    {
        _logger.LogInformation("Publishing event of type {EventType}", typeof(T).Name);
        
        try
        {
            await _publishEndpoint.Publish(@event, cancellationToken);
            _logger.LogInformation("Successfully published event of type {EventType}", typeof(T).Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing event of type {EventType}", typeof(T).Name);
            throw;
        }
    }
}
