using MediatR;
using Shared.Auditing.Interfaces;
using Shared.Auditing.Models;
using System.Text.Json;

namespace Shared.Auditing;

public class AuditBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IAuditPublisher _publisher;

    public AuditBehavior(IAuditPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();

        if (request is IAuditableCommand)
        {
            var audit = new AuditMessage
            {
                CommandName = typeof(TRequest).Name,
                Payload = JsonSerializer.Serialize(request),
                ExecutedBy = "TODO: GetUserId",
                Service = "IdentityService"
            };

            await _publisher.PublishAsync(audit);
        }

        return response;
    }
}
