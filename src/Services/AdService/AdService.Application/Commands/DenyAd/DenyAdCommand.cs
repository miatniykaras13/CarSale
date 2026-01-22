using AdService.Contracts.Ads.Default;

namespace AdService.Application.Commands.DenyAd;

public record DenyAdCommand(Guid AdId, Guid ModeratorId, ModerationResultDto ModerationResult) : ICommand<UnitResult<List<Error>>>;