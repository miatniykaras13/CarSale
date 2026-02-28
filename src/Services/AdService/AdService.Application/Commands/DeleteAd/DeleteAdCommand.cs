namespace AdService.Application.Commands.DeleteAd;

public record DeleteAdCommand(Guid AdId, Guid UserId) : ICommand<UnitResult<List<Error>>>;

