using Microsoft.Extensions.Logging;
using NotificationService.Application.Abstractions;
using NotificationService.Application.Models;

namespace NotificationService.Application.Senders;

public class EmailNotificationSender : IEmailNotificationSender, INotificationSender
{
    private readonly ILogger<EmailNotificationSender> _logger;

    public EmailNotificationSender(ILogger<EmailNotificationSender> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(NotificationMessage message, CancellationToken cancellationToken = default)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        _logger.LogInformation(
            "[EMAIL SENT] User: {UserId}, Type: {Type}, Subject: {Subject}, Body: {Body}",
            message.UserId,
            message.Type,
            message.Metadata?.GetValueOrDefault("subject") ?? "No subject",
            message.Data?.ToString() ?? "No body");

        return Task.CompletedTask;
    }
}
