using Shared.Auditing.Interfaces;
using Shared.Auditing.Models;
using System.Text.Json;

namespace Shared.Auditing.Publishers;

public class ConsoleAuditPublisher : IAuditPublisher
{
    public Task PublishAsync(AuditMessage message)
    {
        Console.WriteLine("[AUDIT_QUEUE] " + JsonSerializer.Serialize(message));
        return Task.CompletedTask;
    }
}
