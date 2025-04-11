using NotificationService.Application.Models;

namespace NotificationService.Application.Abstractions;

/// <summary>
/// Represents a service that can send email notifications
/// </summary>
public interface IEmailNotificationSender : INotificationSender
{
    new Task SendAsync(NotificationMessage message, CancellationToken cancellationToken = default);
}
