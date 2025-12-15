using AdService.Application.Abstractions.Data;
using AdService.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace AdService.Application.Commands.ExpireAds;

public class ExpireAdsHandler(
    IAppDbContext dbContext,
    ILogger<ExpireAdsHandler> logger) : ICommandHandler<ExpireAdsCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(ExpireAdsCommand request, CancellationToken cancellationToken)
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
            return Result.Failure<Unit>(string.Empty);
        }

        return Result.Success(Unit.Value);
    }
}