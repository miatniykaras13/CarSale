using AdService.Contracts.Ads.Default;

namespace AdService.Application.Commands.ArchiveAd;

public record ArchiveAdCommand(Guid AdId, Guid UserId) : ICommand<UnitResult<List<Error>>>;