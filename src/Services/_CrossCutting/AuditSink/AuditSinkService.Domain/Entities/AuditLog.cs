using Shared.Domain;

namespace AuditSinkService.Domain.Entities;

public class AuditLog : BaseEntity
{
    public DateTime Timestamp { get; private set; }
    public string? UserId { get; private set; }
    public string Action { get; private set; } = null!;
    public string ServiceName { get; private set; } = null!;
    public string SerializedPayload { get; private set; } = null!;

    private AuditLog() { } // For EF Core

    public AuditLog(string action, string serviceName, string serializedPayload, string? userId = null)
    {
        Timestamp = DateTime.UtcNow;
        Action = action;
        ServiceName = serviceName;
        SerializedPayload = serializedPayload;
        UserId = userId;
    }
}
