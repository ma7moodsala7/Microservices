using AuditSinkService.Domain.Entities;
using AuditSinkService.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSinkService.Application.Features.PersistAuditLog;

public class PersistAuditLogCommandHandler : IRequestHandler<PersistAuditLogCommand, Guid>
{
    private readonly AuditDbContext _dbContext;
    private readonly ILogger<PersistAuditLogCommandHandler> _logger;

    public PersistAuditLogCommandHandler(AuditDbContext dbContext, ILogger<PersistAuditLogCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Guid> Handle(PersistAuditLogCommand request, CancellationToken cancellationToken)
    {
        var auditLog = new AuditLog(
            action: request.AuditMessage.CommandName,
            serviceName: request.AuditMessage.Service,
            serializedPayload: request.AuditMessage.Payload,
            userId: request.AuditMessage.ExecutedBy
        );

        _dbContext.AuditLogs.Add(auditLog);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Audit log persisted. ID: {Id}, Action: {Action}, Service: {Service}",
            auditLog.Id, auditLog.Action, auditLog.ServiceName);

        return auditLog.Id;
    }
}
