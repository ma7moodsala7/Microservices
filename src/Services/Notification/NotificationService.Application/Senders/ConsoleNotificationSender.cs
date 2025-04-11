using System.Text.Json;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Abstractions;
using NotificationService.Application.Models;

namespace NotificationService.Application.Senders;

public class ConsoleNotificationSender : INotificationSender
{
    private readonly ILogger<ConsoleNotificationSender> _logger;

    public ConsoleNotificationSender(ILogger<ConsoleNotificationSender> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(NotificationMessage message, CancellationToken cancellationToken = default)
    {
        var dataJson = message.Data != null ? JsonSerializer.Serialize(message.Data) : "null";
        var metadataJson = JsonSerializer.Serialize(message.Metadata);

        _logger.LogInformation(
            "[CONSOLE] Notification Details:\n" +
            "UserId: {UserId}\n" +
            "Channel: {Channel}\n" +
            "Type: {Type}\n" +
            "Data: {Data}\n" +
            "Metadata: {Metadata}\n" +
            "CreatedAt: {CreatedAt}",
            message.UserId,
            message.Channel,
            message.Type,
            dataJson,
            metadataJson,
            message.CreatedAt);

        return Task.CompletedTask;
    }
}
