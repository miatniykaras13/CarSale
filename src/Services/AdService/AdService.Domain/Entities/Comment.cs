using CSharpFunctionalExtensions;

namespace AdService.Domain.Entities;

public class Comment : BuildingBlocks.DDD.Abstractions.Entity<int>
{
    public const int MAX_MESSAGE_LENGTH = 500;

    public string Message { get; private set; }

    public Guid AdId { get; private set; }

    private Comment(string message, Guid adId)
    {
        Message = message;
        AdId = adId;
    }


    public static Result<Comment, Error> Create(string message, Guid adId)
    {
        if (message.Length > MAX_MESSAGE_LENGTH)
        {
            return Result.Failure<Comment, Error>(Error.Domain(
                "comment.message.is.conflict",
                $"Comment's name must be less or equal to {MAX_MESSAGE_LENGTH} characters."));
        }

        Comment com = new(message, adId);
        return Result.Success<Comment, Error>(com);
    }
}