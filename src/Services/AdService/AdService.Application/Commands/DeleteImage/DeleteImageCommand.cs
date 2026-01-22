namespace AdService.Application.Commands.DeleteImage;

public record DeleteImageCommand(Guid AdId, Guid ImageId, Guid UserId) : ICommand<UnitResult<List<Error>>>;