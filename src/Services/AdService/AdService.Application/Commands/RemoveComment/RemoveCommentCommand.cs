namespace AdService.Application.Commands.RemoveComment;

public record RemoveCommentCommand(Guid UserId, Guid AdId) : ICommand<UnitResult<List<Error>>>;