using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProfileService.Application.Abstractions.IdentityProvider;
using ProfileService.Infrastructure.Keycloak.Services;

namespace ProfileService.Infrastructure.Keycloak;

public static class DependencyInjection
{
    public static IServiceCollection AddKeycloakInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));

        services.AddHttpClient<IAdminKeycloakService, AdminKeycloakService>((provider, client) =>
        {
            var options = provider.GetRequiredService<IOptions<KeycloakOptions>>().Value;
            client.BaseAddress = new Uri(options.Endpoint.TrimEnd('/') + "/");
        });

        return services;
    }
}