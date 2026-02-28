namespace AdService.Application.Commands.UpdateComment;

public record UpdateCommentCommand(Guid UserId, Guid AdId, string Message) : ICommand<UnitResult<List<Error>>>;

