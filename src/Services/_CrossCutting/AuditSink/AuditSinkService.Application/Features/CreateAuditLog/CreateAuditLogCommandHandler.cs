using AuditSinkService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AuditSinkService.Persistence;

namespace AuditSinkService.Application.Features.CreateAuditLog;

public class CreateAuditLogCommandHandler : IRequestHandler<CreateAuditLogCommand, Guid>
{
    private readonly AuditDbContext _context;
    private readonly ILogger<CreateAuditLogCommandHandler> _logger;

    public CreateAuditLogCommandHandler(AuditDbContext context, ILogger<CreateAuditLogCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateAuditLogCommand request, CancellationToken cancellationToken)
    {
        var auditLog = new AuditLog(
            request.Action,
            request.ServiceName,
            request.SerializedPayload,
            request.UserId);

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Created audit log: {Action} from {Service} for user {UserId}",
            request.Action, request.ServiceName, request.UserId ?? "anonymous");

        return auditLog.Id;
    }
}
