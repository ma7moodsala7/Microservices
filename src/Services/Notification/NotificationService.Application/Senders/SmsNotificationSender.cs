using System.Text.Json;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Abstractions;
using NotificationService.Application.Models;

namespace NotificationService.Application.Senders;

public class SmsNotificationSender : ISmsNotificationSender, INotificationSender
{
    private readonly ILogger<SmsNotificationSender> _logger;

    public SmsNotificationSender(ILogger<SmsNotificationSender> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(NotificationMessage message, CancellationToken cancellationToken = default)
    {
        // Serialize the entire data object as the message
        var messageText = message.Data?.ToString() ?? "No message content";
        
        // In a real implementation, this would use an SMS provider SDK
        _logger.LogInformation(
            "[SMS SENT] User: {UserId}, Type: {Type}, Message: {Data}",
            message.UserId,
            message.Type,
            message.Data);

        // Simulate slight delay as real SMS APIs would have latency
        return Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
    }
}
