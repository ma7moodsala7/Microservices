using NotificationService.Application.Models;

namespace NotificationService.Application.Abstractions;

public interface INotificationSender
{
    Task SendAsync(NotificationMessage message, CancellationToken cancellationToken = default);
}

// Marker interfaces for specific sender types
public interface IEmailSender : INotificationSender { }
public interface ISmsSender : INotificationSender { }
public interface IWebhookSender : INotificationSender { }
