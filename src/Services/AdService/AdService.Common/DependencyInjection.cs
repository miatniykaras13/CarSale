using AdService.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdService.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddAdServiceOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FileStorageOptions>(configuration.GetSection("FileStorage"));
        services.Configure<AdExpirationOptions>(configuration.GetSection("AdExpiration"));
        return services;
    }
}