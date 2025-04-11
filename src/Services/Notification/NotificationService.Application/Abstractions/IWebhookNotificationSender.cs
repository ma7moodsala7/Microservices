using NotificationService.Application.Models;

namespace NotificationService.Application.Abstractions;

/// <summary>
/// Represents a service that can send webhook notifications
/// </summary>
public interface IWebhookNotificationSender : INotificationSender
{
    new Task SendAsync(NotificationMessage message, CancellationToken cancellationToken = default);
}
