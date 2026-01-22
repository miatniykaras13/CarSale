using AdService.Contracts.Ads.Default;

namespace AdService.Application.Commands.PauseAd;

public record PauseAdCommand(Guid AdId, Guid UserId) : ICommand<UnitResult<List<Error>>>;