using AdService.Application.Abstractions.AutoCatalog;
using Microsoft.Extensions.DependencyInjection;

namespace AdService.Infrastructure.AutoCatalog;

public static class DependencyInjection
{
    public static IServiceCollection AddAutoCatalogCommunication(this IServiceCollection services)
    {
        services.AddScoped<IAutoCatalogClient, AutoCatalogClient>();
        return services;
    }
}