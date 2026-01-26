using AdService.Contracts.Ads.Extended;

namespace AdService.Application.Queries.GetAdById;

public record GetAdByIdQuery(Guid AdId, Guid? UserId) : ICommand<Result<AdDto, List<Error>>>;