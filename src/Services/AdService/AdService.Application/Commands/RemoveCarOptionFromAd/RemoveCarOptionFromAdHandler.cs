using AdService.Application.Abstractions.Data;

namespace AdService.Application.Commands.RemoveCarOptionFromAd;

public class RemoveCarOptionFromAdCommandHandler(IAppDbContext dbContext)
    : ICommandHandler<RemoveCarOptionFromAdCommand, UnitResult<List<Error>>>
{
    public async Task<UnitResult<List<Error>>> Handle(RemoveCarOptionFromAdCommand command, CancellationToken ct)
    {
        var ad = await dbContext.Ads
            .Include(a => a.CarOptions)
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
                $"Authenticated user does not own the ad"));
        }

        var carOption = await dbContext.CarOptions.FindAsync([command.CarOptionId], ct).AsTask();

        if (carOption is null)
        {
            return UnitResult.Failure<List<Error>>(Error.NotFound(
                "car_option",
                $"Car option with id {command.CarOptionId} not found"));
        }

        var removeOptionResult = ad.RemoveCarOption(carOption);
        if (removeOptionResult.IsFailure)
            return UnitResult.Failure<List<Error>>(removeOptionResult.Error);

        await dbContext.SaveChangesAsync(ct);
        return UnitResult.Success<List<Error>>();
    }
}