using AdService.Contracts.Ads.Extended;

namespace AdService.Application.Commands.GetPublishedAdById;

public record GetPublishedAdByIdCommand(Guid AdId) : ICommand<Result<AdDto, Error>>;