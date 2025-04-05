using MassTransit;
using Shared.Messaging.Events;

namespace IdentityService.API.Consumers;

public class UserPingedConsumer : IConsumer<UserPingedIntegrationEvent>
{
    public Task Consume(ConsumeContext<UserPingedIntegrationEvent> context)
    {
        var message = context.Message;
        Console.WriteLine($"[EVENT RECEIVED] User: {message.UserId}, Message: {message.Message}");
        return Task.CompletedTask;
    }
}
