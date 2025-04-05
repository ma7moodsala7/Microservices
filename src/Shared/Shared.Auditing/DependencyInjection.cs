using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Auditing.Interfaces;
using Shared.Auditing.Publishers;

namespace Shared.Auditing;

public static class DependencyInjection
{
    public static IServiceCollection AddAuditing(this IServiceCollection services)
    {
        services.AddTransient<IAuditPublisher, ConsoleAuditPublisher>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuditBehavior<,>));
        
        return services;
    }
}
