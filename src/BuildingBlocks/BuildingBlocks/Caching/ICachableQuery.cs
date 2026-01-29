using BuildingBlocks.CQRS;

namespace BuildingBlocks.Caching;

public interface ICachableQuery<out TResponse> : IQuery<TResponse>
    where TResponse : notnull
{
    string CacheKey { get; }
}