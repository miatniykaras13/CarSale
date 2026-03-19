using AdService.Application.Builders;
using AdService.Domain.Aggregates;
using AdService.Domain.Events;
using Microsoft.Extensions.Caching.Hybrid;

namespace AdService.Application.EventHandlers.Domain;

public class AdUpdatedEventHandler(HybridCache cache) : INotificationHandler<AdUpdatedEvent>
{
    public async Task Handle(AdUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var adExtendedCacheKey = CacheKeyBuilder.Build(
            nameof(Ad),
            notification.Ad.Id.ToString());

        var adListItemCacheKey = CacheKeyBuilder.BuildListItem(
            nameof(Ad),
            notification.Ad.Id.ToString());

        await cache.RemoveAsync(adExtendedCacheKey, cancellationToken);

        await cache.RemoveAsync(adListItemCacheKey, cancellationToken);
    }
}