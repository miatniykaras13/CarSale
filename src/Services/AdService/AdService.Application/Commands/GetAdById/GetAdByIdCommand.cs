using AdService.Contracts.Ads.Extended;

namespace AdService.Application.Commands.GetAdById;

public record GetAdByIdCommand(Guid AdId, Guid? UserId) : ICommand<Result<AdDto, List<Error>>>;