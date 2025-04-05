namespace Shared.Auditing.Models;

public class AuditMessage
{
    public string CommandName { get; set; } = null!;
    public string ExecutedBy { get; set; } = null!;
    public string Payload { get; set; } = null!;
    public string Service { get; set; } = null!;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
