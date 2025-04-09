using System.Text.Json;

namespace NotificationService.Application.Models;

public class NotificationMessage
{
    public Guid UserId { get; set; }
    public string Channel { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public JsonDocument? Data { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
