namespace AdService.Application.Commands.RemoveCarOptionFromAd;

public record RemoveCarOptionFromAdCommand(Guid UserId, Guid AdId, int CarOptionId) : ICommand<UnitResult<List<Error>>>;