namespace AdService.Application.Commands.AddCarOptionToAd;

public record AddCarOptionToAdCommand(Guid UserId, Guid AdId, int CarOptionId) : ICommand<UnitResult<List<Error>>>;