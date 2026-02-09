using AdService.Application.Abstractions.AutoCatalog;
using AdService.Infrastructure.AutoCatalog.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdService.Infrastructure.AutoCatalog;

public static class DependencyInjection
{
    public static IServiceCollection AddAutoCatalogCommunication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<IAutoCatalogClient, AutoCatalogClient>();
        services.Configure<AutoCatalogOptions>(configuration.GetSection("AutoCatalog"));
        return services;
    }
}