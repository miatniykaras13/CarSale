using AdService.Application.Builders;
using AdService.Contracts.Ads.Default;
using AdService.Domain.Entities;
using BuildingBlocks.Caching;

namespace AdService.Application.Queries.GetCarOptionsFromAd;

public record GetCarOptionsFromAdQuery(Guid? UserId, Guid AdId) : ICachableQuery<Result<IEnumerable<CarOptionDto>, List<Error>>>
{
    public string CacheKey => CacheKeyBuilder.BuildIndex("ad:car_options", AdId.ToString());
}