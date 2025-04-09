using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Abstractions;
using NotificationService.Application.Models;

namespace NotificationService.Application.Senders;

public class WebhookNotificationSender : IWebhookNotificationSender
{
    private readonly ILogger<WebhookNotificationSender> _logger;
    private readonly HttpClient _httpClient;

    public WebhookNotificationSender(ILogger<WebhookNotificationSender> logger, HttpClient httpClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task SendAsync(NotificationMessage message, CancellationToken cancellationToken = default)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        // Extract webhook URL from metadata
        var webhookUrl = message.Metadata?.GetValueOrDefault("webhookUrl");
        if (string.IsNullOrEmpty(webhookUrl))
        {
            throw new InvalidOperationException("Webhook URL not provided in metadata");
        }

        try
        {
            // Prepare webhook payload
            var payload = new
            {
                userId = message.UserId,
                type = message.Type,
                data = message.Data,
                metadata = message.Metadata,
                timestamp = DateTimeOffset.UtcNow.ToString("o") // ISO 8601
            };

            // Send HTTP POST request
            var response = await _httpClient.PostAsJsonAsync(webhookUrl, payload, cancellationToken);
            
            // Ensure successful response
            response.EnsureSuccessStatusCode();

            _logger.LogInformation(
                "[WEBHOOK SENT] User: {UserId}, Type: {Type}, URL: {WebhookUrl}, Status: {StatusCode}",
                message.UserId,
                message.Type,
                webhookUrl,
                response.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "Failed to send webhook notification. User: {UserId}, Type: {Type}, URL: {WebhookUrl}",
                message.UserId,
                message.Type,
                webhookUrl);
            throw;
        }
    }
}
