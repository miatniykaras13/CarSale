namespace AdService.Application.Commands.SubmitAd;

public record SubmitAdCommand(Guid AdId, Guid UserId) : ICommand<UnitResult<List<Error>>>;