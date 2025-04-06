using MediatR;

namespace AuditSinkService.Application.Features.CreateAuditLog;

public record CreateAuditLogCommand(
    string Action,
    string ServiceName,
    string SerializedPayload,
    string? UserId = null) : IRequest<Guid>;
