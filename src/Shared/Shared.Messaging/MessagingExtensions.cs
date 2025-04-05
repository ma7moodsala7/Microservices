using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Messaging;

public static class MessagingExtensions
{
    public static IServiceCollection AddSharedMassTransit(
        this IServiceCollection services,
        params Type[] consumerTypes)
    {
        services.AddMassTransit(x =>
        {
            foreach (var consumer in consumerTypes)
            {
                x.AddConsumer(consumer);
            }

            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        // Register message publisher
        services.AddScoped<IMessagePublisher, MassTransitPublisher>();

        return services;
    }
}
