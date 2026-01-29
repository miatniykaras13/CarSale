using AdService.Application.Builders;
using AdService.Contracts.Ads.Extended;
using AdService.Domain.Aggregates;
using BuildingBlocks.Caching;

namespace AdService.Application.Queries.GetAdById;

public record GetAdByIdQuery(Guid AdId, Guid? UserId) : ICachableQuery<Result<AdDto, List<Error>>>
{
    public string CacheKey => CacheKeyBuilder.Build(nameof(Ad), AdId.ToString());
}