using AdService.Application.Abstractions.Data;

namespace AdService.Application.Commands.RestoreAd;

public class RestoreAdHandler(IAppDbContext dbContext)
    : ICommandHandler<RestoreAdCommand, UnitResult<List<Error>>>
{
    public async Task<UnitResult<List<Error>>> Handle(RestoreAdCommand command, CancellationToken ct)
    {
        var ad = await dbContext.Ads.FindAsync([command.AdId], ct);

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

        var restoreResult = ad.Restore();
        if (restoreResult.IsFailure)
            return UnitResult.Failure<List<Error>>(restoreResult.Error);

        await dbContext.SaveChangesAsync(ct);
        return UnitResult.Success<List<Error>>();
    }
}