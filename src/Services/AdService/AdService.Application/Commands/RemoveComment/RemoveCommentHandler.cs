using AdService.Application.Abstractions.Data;

namespace AdService.Application.Commands.RemoveComment;

public class RemoveCommentHandler(IAppDbContext dbContext)
    : ICommandHandler<RemoveCommentCommand, UnitResult<List<Error>>>
{
    public async Task<UnitResult<List<Error>>> Handle(RemoveCommentCommand command, CancellationToken ct)
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

        var removeResult = ad.RemoveComment();
        if (removeResult.IsFailure)
            return UnitResult.Failure<List<Error>>(removeResult.Error);

        await dbContext.SaveChangesAsync(ct);
        return UnitResult.Success<List<Error>>();
    }
}