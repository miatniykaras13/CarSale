using System.Text.Json;
using AdService.Application.Options;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Backplane.StackExchangeRedis;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace AdService.Infrastructure.Redis;

public static class DependencyInjection
{
    public static IServiceCollection AddHybridCachingWithFusionAndRedis(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis");

        services
            .AddFusionCache()
            .WithDefaultEntryOptions(o =>
            {
                o.DistributedCacheDuration = configuration.GetValue<TimeSpan>("Cache:DistributedAbsoluteExpiration");
                o.MemoryCacheDuration = configuration.GetValue<TimeSpan>("Cache:MemoryAbsoluteExpiration");
            })
            .WithSerializer(
                new FusionCacheSystemTextJsonSerializer(new JsonSerializerOptions { IncludeFields = true, WriteIndented = true }))
            .WithBackplane(new RedisBackplane(new RedisBackplaneOptions { Configuration = redisConnectionString }))
            .WithDistributedCache(new RedisCache(new RedisCacheOptions() { Configuration = redisConnectionString }))
            .AsHybridCache();

        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConnectionString!));
        services.AddSingleton(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase());

        services.Configure<CacheOptions>(configuration.GetSection("Cache"));
        return services;
    }
}
