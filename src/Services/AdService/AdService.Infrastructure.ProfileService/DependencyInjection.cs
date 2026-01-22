using AdService.Application.Abstractions.UserProfiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdService.Infrastructure.ProfileService;

public static class DependencyInjection
{
    public static IServiceCollection AddProfileServiceCommunication(this IServiceCollection services)
    {
        services.AddScoped<IProfileServiceClient, ProfileServiceClient>();
        return services;
    }
}