using AdService.Application.Abstractions.Helpers;
using AdService.Infrastructure.Core.BackgroundServices;
using AdService.Infrastructure.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace AdService.Infrastructure.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddBackgroundServices(
        this IServiceCollection services)
    {
        services.AddHostedService<AdExpirationCheckerService>();
        services.AddSingleton<IMergePatchHelper, MergePatchHelper>();
        return services;
    }
}