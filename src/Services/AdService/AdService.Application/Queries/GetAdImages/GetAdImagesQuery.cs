using AdService.Contracts.Ads.Default;

namespace AdService.Application.Queries.GetAdImages;

public record GetAdImagesQuery(Guid AdId, Guid? UserId) : IQuery<Result<IEnumerable<AdImageDto>, List<Error>>>;


