/*using System.Linq.Expressions;
using AdService.Application.Abstractions.Data;
using AdService.Application.Queries;
using AdService.Domain.Aggregates;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace AdService.Infrastructure.Postgres.Data.Repositories;

public class AdsEfRepository(AppDbContext context) : IAdsRepository
{
    public async Task<Result<Ad, Error>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var ad = await context.Ads
            .Include(a => a.CarOptions)
            .Include(a => a.Comment)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (ad is null)
            return Result.Failure<Ad, Error>(Error.NotFound("ad", $"Ad with id {id} not found"));

        return ad;
    }
    

    public async Task<Result<List<Ad>, Error>> GetAllAsync(
        AdFilter filter,
        SortParameters sortParameters,
        PageParameters pageParameters,
        CancellationToken cancellationToken = default)
    {
        var ads = await context.Ads.ToListAsync(cancellationToken);
        return ads;
    }

    public async Task<Result<List<Ad>, Error>> GetAllFullAsync(
        AdFilter filter,
        SortParameters sortParameters,
        PageParameters pageParameters,
        CancellationToken cancellationToken = default)
    {
        var ads = await context.Ads
            .Include(a => a.CarOptions)
            .Include(a => a.Comment)
            .ToListAsync(cancellationToken);
        return ads;
    }

    public async Task<Result<Guid, Error>> AddAsync(Ad ad, CancellationToken cancellationToken = default)
    {
        await context.Ads.AddAsync(ad, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<Guid, Error>(ad.Id);
    }

    public void Attach(Ad entity) => context.Ads.Attach(entity);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await context.SaveChangesAsync(cancellationToken);

    public async Task<Result<Guid, Error>> UpdateAsync(Ad ad, CancellationToken cancellationToken = default)
    {
        context.Ads.Update(ad);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<Guid, Error>(ad.Id);
    }

    public async Task<UnitResult<Error>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var adResult = await GetByIdAsync(id, cancellationToken);
        if (adResult.IsFailure)
            return adResult.Error;

        context.Ads.Remove(adResult.Value);

        await context.SaveChangesAsync(cancellationToken);
        return UnitResult.Success<Error>();
    }
}*/