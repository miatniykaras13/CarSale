using AdService.Application.Abstractions.Data;
using AdService.Domain.ValueObjects;

namespace AdService.Application.Commands.ArchiveAd;

public class ArchiveAdCommandHandler(IAppDbContext dbContext)
    : ICommandHandler<ArchiveAdCommand, UnitResult<List<Error>>>
{
    public async Task<UnitResult<List<Error>>> Handle(ArchiveAdCommand command, CancellationToken ct)
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

        var archiveResult = ad.Archive();

        if (archiveResult.IsFailure) return UnitResult.Failure<List<Error>>(archiveResult.Error);

        await dbContext.SaveChangesAsync(ct);

        return UnitResult.Success<List<Error>>();
    }
}