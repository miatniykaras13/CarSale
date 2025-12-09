using AdService.Infrastructure.Core.BackgroundServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdService.Infrastructure.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddBackgroundServices(
        this IServiceCollection services)
    {
        services.AddHostedService<AdExpirationCheckerService>();
        return services;
    }
}