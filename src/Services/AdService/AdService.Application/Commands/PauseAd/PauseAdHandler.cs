using AdService.Application.Abstractions.Data;
using AdService.Domain.ValueObjects;

namespace AdService.Application.Commands.PauseAd;

public class PauseAdCommandHandler(IAppDbContext dbContext) : ICommandHandler<PauseAdCommand, UnitResult<List<Error>>>
{
    public async Task<UnitResult<List<Error>>> Handle(PauseAdCommand command, CancellationToken ct)
    {
        var ad = await dbContext.Ads.FindAsync([command.AdId], ct);

        if (ad is null)
            return UnitResult.Failure<List<Error>>(Error.NotFound("ad", $"Ad with id {command.AdId} not found"));

        if (ad.Seller.SellerId != command.UserId)
        {
            return UnitResult.Failure<List<Error>>(Error.Forbidden(
                "ad",
                $"Authenticated user does not own the ad"));
        }

        var pauseResult = ad.Pause();

        if (pauseResult.IsFailure) return UnitResult.Failure<List<Error>>(pauseResult.Error);

        await dbContext.SaveChangesAsync(ct);

        return UnitResult.Success<List<Error>>();
    }
}