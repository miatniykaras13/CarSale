using AdService.Contracts.Ads.ListItems;
using BuildingBlocks.Application.Paging;

namespace AdService.Application.Queries.GetAds;

public record GetAdsQuery(
    Guid? UserId,
    AdFilter Filter,
    PageParameters PageParameters,
    bool IncludeImageUrls,
    string Sort) : IQuery<Result<AdsPageDto, List<Error>>>
{
    public const string Newest = "newest";
    public const string Oldest = "oldest";
}
