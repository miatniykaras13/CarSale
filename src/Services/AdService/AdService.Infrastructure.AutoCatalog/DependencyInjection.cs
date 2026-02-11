using AdService.Application.Abstractions.AutoCatalog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdService.Infrastructure.AutoCatalog;

public static class DependencyInjection
{
    public static IServiceCollection AddAutoCatalogCommunication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<IAutoCatalogClient, AutoCatalogClient>((client) =>
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("AutoCatalog:Endpoint") ??
                                         throw new InvalidOperationException(
                                             "Configuration key 'AutoCatalog:Endpoint' is missing or empty."));
        });
        return services;
    }
}