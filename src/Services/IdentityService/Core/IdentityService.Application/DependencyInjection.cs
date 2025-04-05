using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Auditing;

namespace IdentityService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddAuditing();
        
        return services;
    }
}
