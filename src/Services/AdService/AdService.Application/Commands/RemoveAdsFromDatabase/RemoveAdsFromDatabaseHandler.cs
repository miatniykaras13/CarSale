using AdService.Application.Abstractions.Data;
using AdService.Application.Options;
using AdService.Domain.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AdService.Application.Commands.RemoveAdsFromDatabase;

public class RemoveAdsFromDatabaseCommandHandler(
    IAppDbContext dbContext,
    IOptions<AdDeletionOptions> options,
    ILogger<RemoveAdsFromDatabaseCommandHandler> logger) : ICommandHandler<RemoveAdsFromDatabaseCommand, Result>
{
    private readonly AdDeletionOptions _deletionOptions = options.Value;

    public async Task<Result> Handle(RemoveAdsFromDatabaseCommand request, CancellationToken cancellationToken)
    {
        var removed = 0;
        var adsToRemove = await dbContext.Ads
            .Where(a => a.Status == AdStatus.DELETED || a.Status == AdStatus.EXPIRED)
            .ToListAsync(cancellationToken);

        foreach (var ad in adsToRemove)
        {
            if (ad.UpdatedAt.Add(_deletionOptions.TimeToRestore) < DateTime.UtcNow)
            {
                dbContext.Ads.Remove(ad);
                removed++;
            }
        }

        logger.LogInformation("Removed {removed} ads from database", removed);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}