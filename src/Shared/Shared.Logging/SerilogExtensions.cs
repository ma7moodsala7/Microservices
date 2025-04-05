using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

namespace Shared.Logging;

public static class SerilogExtensions
{
    public static IHostBuilder UseSharedSerilog(this IHostBuilder hostBuilder)
    {
        return hostBuilder
            .ConfigureServices((context, services) =>
            {
                services.AddOpenTelemetry()
                    .WithTracing(builder => builder
                        .SetResourceBuilder(ResourceBuilder
                            .CreateDefault()
                            .AddService(context.HostingEnvironment.ApplicationName))
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddConsoleExporter());
            })
            .UseSerilog((context, config) =>
            {
                config
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .WriteTo.Console();
            });
    }
}
