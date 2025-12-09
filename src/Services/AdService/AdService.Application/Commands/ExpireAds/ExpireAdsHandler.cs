using AdService.Application.Data;
using AdService.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace AdService.Application.Commands.ExpireAds;

public class ExpireAdsHandler(
    IAppDbContext dbContext,
    ILogger<ExpireAdsHandler> logger) : ICommandHandler<ExpireAdsCommand, ExpireAdsResponse>
{
    public async Task<ExpireAdsResponse> Handle(ExpireAdsCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var expiredAds =
            await dbContext.Ads.Where(a => a.Status == AdStatus.PUBLISHED && a.ExpiresAt <= now)
                .ToListAsync(cancellationToken);

        int errors = 0;

        foreach (var ad in expiredAds)
        {
            var result = ad.Expire();
            if (result.IsFailure)
            {
                logger.LogInformation("Cannot expire ad with id {AdId}, reason: {Error}", ad.Id, result.Error.Message);
                errors++;
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Expired {Count} ads.", expiredAds.Count - errors);

        

        if (errors != 0)
        {
            logger.LogWarning("Couldn't expire {ErrorsCount} ads.", errors);
            return new ExpireAdsResponse(UnitResult.Failure<Unit>(Unit.Value));
        }

        return new ExpireAdsResponse(UnitResult.Success<Unit>());
    }
}