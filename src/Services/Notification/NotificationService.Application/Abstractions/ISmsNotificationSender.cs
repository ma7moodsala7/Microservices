using NotificationService.Application.Models;

namespace NotificationService.Application.Abstractions;

/// <summary>
/// Interface for SMS notification delivery
/// </summary>
public interface ISmsNotificationSender : INotificationSender
{
    // Inherits SendAsync from INotificationSender
    // This is a marker interface to help with dependency injection and message routing
}
