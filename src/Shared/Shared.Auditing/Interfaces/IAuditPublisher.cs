using Shared.Auditing.Models;

namespace Shared.Auditing.Interfaces;

public interface IAuditPublisher
{
    Task PublishAsync(AuditMessage message);
}
