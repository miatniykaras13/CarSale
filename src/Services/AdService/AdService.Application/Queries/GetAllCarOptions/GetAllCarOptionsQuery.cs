using AdService.Application.Builders;
using AdService.Contracts.Ads.Default;
using AdService.Domain.Entities;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Caching;

namespace AdService.Application.Queries.GetAllCarOptions;

public record GetAllCarOptionsQuery(
    CarOptionFilter Filter,
    PageParameters PageParameters) : ICachableQuery<Result<IEnumerable<CarOptionDto>, List<Error>>>
{
    public string CacheKey => CacheKeyBuilder.BuildIndex(
        nameof(CarOption),
        HashCodeBuilder.Build(Filter));
}

