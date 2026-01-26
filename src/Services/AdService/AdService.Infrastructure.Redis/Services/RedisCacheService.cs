using System.Text.Json;
using AdService.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace AdService.Infrastructure.Redis.Services;

public class RedisCacheService(IDistributedCache cache) : ICacheService
{
    public async Task<T?> GetDataAsync<T>(string key, CancellationToken ct = default)
    {
        var cachedValue = await cache.GetStringAsync(key, ct);

        if (cachedValue == null)
            return default;

        T? value = JsonSerializer.Deserialize<T>(cachedValue);

        return value;
    }

    public async Task SetDataAsync<T>(string key, T value, CancellationToken ct = default)
    {
        await cache.SetStringAsync(key, JsonSerializer.Serialize(value), ct);
    }

    public async Task RemoveDataAsync(string key, CancellationToken ct = default)
    {
        await cache.RemoveAsync(key, ct);
    }
}