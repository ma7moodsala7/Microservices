using Microsoft.Extensions.Logging;
using NotificationService.Application.Abstractions;
using NotificationService.Application.Models;
using NotificationService.Application.Senders;
using NotificationService.Application.Dispatching;

namespace NotificationService.Application.Dispatching;

public class NotificationDispatcher
{
    private readonly IEnumerable<INotificationSender> _senders;
    private readonly ILogger<NotificationDispatcher> _logger;

    public NotificationDispatcher(
        IEnumerable<INotificationSender> senders,
        ILogger<NotificationDispatcher> logger)
    {
        _senders = senders.ToList();
        _logger = logger;
        
        // Log registered senders for debugging
        foreach (var sender in _senders)
        {
            _logger.LogInformation("Registered sender: {SenderType}", sender.GetType().Name);
        }
        if (senders == null) throw new ArgumentNullException(nameof(senders));
        if (logger == null) throw new ArgumentNullException(nameof(logger));
    }

    public async Task DispatchAsync(NotificationMessage message, CancellationToken cancellationToken = default)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        // Log available senders for debugging
        foreach (var s in _senders)
        {
            _logger.LogInformation("Available sender: {SenderType} implementing {Interfaces}", 
                s.GetType().Name,
                string.Join(", ", s.GetType().GetInterfaces().Select(i => i.Name)));
        }

        if (!_senders.Any())
        {
            _logger.LogWarning("No notification senders registered");
            return;
        }

        // Select sender based on channel
        INotificationSender? sender = message.Channel?.ToLowerInvariant() switch
        {
            "sms" => _senders.OfType<ISmsNotificationSender>().FirstOrDefault(),
            "email" => _senders.OfType<IEmailNotificationSender>().FirstOrDefault(),
            "webhook" => _senders.OfType<IWebhookNotificationSender>().FirstOrDefault(),
            "console" => _senders.OfType<INotificationSender>().FirstOrDefault(s => s.GetType() == typeof(ConsoleNotificationSender)),
            _ => null
        };

        if (sender == null)
        {
            _logger.LogError("No sender found for channel: {Channel}", message.Channel);
            throw new InvalidOperationException($"No sender found for channel: {message.Channel}");
        }
        
        try
        {
            await sender.SendAsync(message, cancellationToken);
            _logger.LogInformation(
                "Notification dispatched successfully via {Channel} for user {UserId}", 
                message.Channel,
                message.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to dispatch {Channel} notification for user {UserId}",
                message.Channel,
                message.UserId);
            throw;
        }
    }
}
