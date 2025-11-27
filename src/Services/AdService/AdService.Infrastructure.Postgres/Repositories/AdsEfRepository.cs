using AdService.Domain.Aggregates;
using AdService.Infrastructure.Postgres.Data;
using CSharpFunctionalExtensions;

namespace AdService.Infrastructure.Postgres.Repositories;

public class AdsEfRepository(AppDbContext context)
{
    public async Task<Result<Guid>> AddAsync(Ad ad, CancellationToken cancellationToken)
    {
        await context.Ads.AddAsync(ad, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success(ad.Id);
    }

    public async Task<Result<Guid>> UpdateAsync(Ad ad, CancellationToken cancellationToken)
    {
        context.Ads.Update(ad);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success(ad.Id);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        Result<Ad> adResult = await GetByIdAsync(id, cancellationToken);
        if (adResult.IsFailure)
        {
            return Result.Failure<Guid>(adResult.Error);
        }

        Ad? ad = adResult.Value;
        context.Ads.Remove(ad);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success(ad.Id);
    }

    public async Task<Result<Ad>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        Ad? ad = await context.Ads.FindAsync(id, cancellationToken);
        if (ad == null)
        {
            return Result.Failure<Ad>($"Ad with id {id} not found");
        }

        return Result.Success(ad);
    }
}