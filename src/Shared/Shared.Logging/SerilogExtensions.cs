using Microsoft.Extensions.Hosting;
using Serilog;

namespace Shared.Logging;

public static class SerilogExtensions
{
    public static IHostBuilder UseSharedSerilog(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((context, config) =>
        {
            config
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Console();
        });
    }
}
