using MediatR;
using Shared.Auditing.Models;

namespace AuditSinkService.Application.Features.PersistAuditLog;

public record PersistAuditLogCommand(AuditMessage AuditMessage) : IRequest<Guid>;
