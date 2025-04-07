using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Shared.Logging;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddOpenTelemetrySupport(
        this IServiceCollection services,
        IConfiguration configuration,
        string serviceName)
    {
        services.AddOpenTelemetry()
            .WithTracing(builder =>
            {
                builder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddMassTransitInstrumentation()
                    .SetResourceBuilder(
                        ResourceBuilder.CreateDefault()
                            .AddService(serviceName)
                    )
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(configuration["OpenTelemetry:OtlpEndpoint"] ?? "http://otel-collector:4317");
                    });
            });

        return services;
    }
}
