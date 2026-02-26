using AdService.Application.Builders;
using AdService.Contracts.Ads.ListItems;
using AdService.Domain.Aggregates;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Caching;

namespace AdService.Application.Queries.GetAds;

public record GetAdsQuery(Guid? UserId, AdFilter Filter, PageParameters PageParameters, bool IncludeImageUrls)
    : ICachableQuery<Result<List<AdListItemDto>, List<Error>>>
{
    public string CacheKey => CacheKeyBuilder.BuildIndex(nameof(Ad), Filter.GetHashCode().ToString());
}