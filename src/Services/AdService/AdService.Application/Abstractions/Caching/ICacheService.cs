namespace AdService.Application.Abstractions.Caching;

public interface ICacheService
{
    Task<T?> GetDataAsync<T>(string key, CancellationToken ct = default);

    Task SetDataAsync<T>(string key, T value, CancellationToken ct = default);

    Task RemoveDataAsync(string key, CancellationToken ct = default);
}