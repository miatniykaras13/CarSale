namespace AdService.Application.Commands.RestoreAd;

public record RestoreAdCommand(Guid AdId, Guid UserId) : ICommand<UnitResult<List<Error>>>;

