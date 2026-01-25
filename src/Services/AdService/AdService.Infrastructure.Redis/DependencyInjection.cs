using AdService.Application.Abstractions.Caching;
using AdService.Infrastructure.Redis.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdService.Infrastructure.Redis;

public static class DependencyInjection
{
    public static IServiceCollection AddRedisCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(o =>
        {
            o.Configuration = configuration.GetConnectionString("Redis");
            o.InstanceName = "AdService_";
        });
        services.AddSingleton<ICacheService, RedisCacheService>();
        return services;
    }
}