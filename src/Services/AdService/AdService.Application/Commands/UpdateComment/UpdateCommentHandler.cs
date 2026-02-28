using AdService.Application.Abstractions.Data;
using AdService.Domain.Entities;

namespace AdService.Application.Commands.UpdateComment;

public class UpdateCommentHandler(IAppDbContext dbContext)
    : ICommandHandler<UpdateCommentCommand, UnitResult<List<Error>>>
{
    public async Task<UnitResult<List<Error>>> Handle(UpdateCommentCommand command, CancellationToken ct)
    {
        var ad = await dbContext.Ads
            .Include(a => a.Comment)
            .FirstOrDefaultAsync(a => a.Id == command.AdId, ct);

        if (ad is null)
        {
            return UnitResult.Failure<List<Error>>(Error.NotFound(
                "ad",
                $"Ad with id {command.AdId} not found"));
        }

        if (ad.Seller.SellerId != command.UserId)
        {
            return UnitResult.Failure<List<Error>>(Error.Forbidden(
                "ad",
                "Authenticated user does not own the ad"));
        }

        Comment? comment = ad.Comment;

        if (comment is null)
        {
            var commentResult = Comment.Create(command.Message, ad.Id);
            if (commentResult.IsFailure)
                return UnitResult.Failure<List<Error>>(commentResult.Error);
            comment = commentResult.Value;
        }
        else
        {
            comment.Update(command.Message);
        }

        var updateResult = ad.UpdateComment(comment);
        if (updateResult.IsFailure)
            return UnitResult.Failure<List<Error>>(updateResult.Error);

        await dbContext.SaveChangesAsync(ct);
        return UnitResult.Success<List<Error>>();
    }
}