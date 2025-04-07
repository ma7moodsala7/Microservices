using Microsoft.Extensions.Configuration;

namespace Shared.Settings;

public static class SharedConfigurationExtensions
{
    public static IConfigurationBuilder AddSharedSettings(this IConfigurationBuilder builder)
    {
        var sharedConfigPath = Path.Combine(
            AppContext.BaseDirectory,
            "appsettings.shared.json"
        );

        return builder.AddJsonFile(sharedConfigPath, optional: false, reloadOnChange: true);
    }
}
