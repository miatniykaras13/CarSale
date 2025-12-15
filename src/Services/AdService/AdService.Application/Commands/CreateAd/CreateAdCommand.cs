namespace AdService.Application.Commands.CreateAd;

public record CreateAdCommand(Guid UserId) : ICommand<Result<CreateAdResponse, List<Error>>>;