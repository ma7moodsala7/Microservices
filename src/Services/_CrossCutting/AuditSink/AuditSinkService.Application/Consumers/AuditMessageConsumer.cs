using AuditSinkService.Application.Features.PersistAuditLog;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Auditing.Models;

namespace AuditSinkService.Application.Consumers;

public class AuditMessageConsumer : IConsumer<AuditMessage>
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuditMessageConsumer> _logger;

    public AuditMessageConsumer(IMediator mediator, ILogger<AuditMessageConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AuditMessage> context)
    {
        var message = context.Message;
        _logger.LogInformation(
            "Received audit message. Command: {Command}, Service: {Service}",
            message.CommandName, message.Service);

        var command = new PersistAuditLogCommand(message);
        await _mediator.Send(command);
    }
}
