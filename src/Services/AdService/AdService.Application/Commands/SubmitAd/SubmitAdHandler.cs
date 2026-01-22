using AdService.Application.Abstractions.Data;

namespace AdService.Application.Commands.SubmitAd;

public class SubmitAdCommandHandler(IAppDbContext dbContext) : ICommandHandler<SubmitAdCommand, UnitResult<List<Error>>>
{
    public async Task<UnitResult<List<Error>>> Handle(SubmitAdCommand command, CancellationToken ct)
    {
        var userId = command.UserId;

        var ad = await dbContext.Ads.FindAsync([command.AdId], ct);

        if (ad is null)
            return UnitResult.Failure<List<Error>>(Error.NotFound("ad", $"Ad with id {command.AdId} not found"));

        if (ad.Seller.SellerId != userId)
        {
            return UnitResult.Failure<List<Error>>(Error.Forbidden(
                "ad",
                $"Authenticated user does not own the ad"));
        }

        var submitResult = ad.Submit();

        if (submitResult.IsFailure)
            return UnitResult.Failure<List<Error>>(submitResult.Error);

        await dbContext.SaveChangesAsync(ct);

        return UnitResult.Success<List<Error>>();
    }
}