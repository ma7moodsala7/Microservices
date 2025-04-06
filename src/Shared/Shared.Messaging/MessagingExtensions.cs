using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Shared.Messaging;

public static class MessagingExtensions
{
    public static IServiceCollection AddSharedMassTransit(
        this IServiceCollection services,
        params Type[] consumerTypes)
    {
        services.AddMassTransit(x =>
        {
            // Add consumers with explicit queue configuration
            foreach (var consumer in consumerTypes)
            {
                x.AddConsumer(consumer);
            }

            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) =>
            {
                var configuration = context.GetRequiredService<IConfiguration>();
                var host = configuration["RabbitMQ:Host"] ?? "localhost";
                var username = configuration["RabbitMQ:Username"] ?? "guest";
                var password = configuration["RabbitMQ:Password"] ?? "guest";

                cfg.Host(host, "/", h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        // Register message publisher
        services.AddScoped<IMessagePublisher, MassTransitPublisher>();

        return services;
    }
}
