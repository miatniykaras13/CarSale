using AdService.Application.Abstractions.Data;
using AdService.Application.Builders;
using AdService.Application.Options;
using AdService.Contracts.Ads.Default;
using AdService.Domain.Entities;
using AdService.Domain.Enums;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AdService.Application.Queries.GetCarOptionsFromAd;

public class GetCarOptionsFromAdQueryHandler(
    IAppDbContext dbContext,
    HybridCache cache,
    ILogger<GetCarOptionsFromAdQueryHandler> logger,
    IOptions<CacheOptions> options)
    : IQueryHandler<GetCarOptionsFromAdQuery, Result<IEnumerable<CarOptionDto>, List<Error>>>
{
    private readonly CacheOptions cacheOptions = options.Value;

    public async Task<Result<IEnumerable<CarOptionDto>, List<Error>>> Handle(
        GetCarOptionsFromAdQuery query,
        CancellationToken ct)
    {
        List<CarOptionDto> carOptionDtos = [];
        var isUserAuthorized = query.UserId is not null;

        var carOptionIds = await cache.GetOrCreateAsync<List<int>?>(
            query.CacheKey,
            factory: null!,
            options: new HybridCacheEntryOptions() { Flags = HybridCacheEntryFlags.DisableUnderlyingData },
            cancellationToken: ct);

        if (carOptionIds is not null)
        {
            foreach (var carOptionId in carOptionIds)
            {
                var carOptionCacheKey = CacheKeyBuilder.Build(nameof(CarOption), carOptionId.ToString());
                var carOption = await cache.GetOrCreateAsync<CarOptionDto?>(
                    carOptionCacheKey,
                    factory: null!,
                    options: new HybridCacheEntryOptions() { Flags = HybridCacheEntryFlags.DisableUnderlyingData },
                    cancellationToken: ct);

                if (carOption is null)
                {
                    var carOptionFromDb = await dbContext.CarOptions.FindAsync([carOptionId], ct);
                    if (carOptionFromDb is null)
                    {
                        logger.LogWarning("Car option with id {CarOptionId} not found. Skipping.", carOptionId);
                        continue;
                    }

                    carOption = new CarOptionDto(
                        carOptionFromDb.Id,
                        carOptionFromDb.OptionType.ToString(),
                        carOptionFromDb.Name,
                        carOptionFromDb.TechnicalName);
                    await cache.SetAsync(
                        carOptionCacheKey,
                        carOption,
                        options: new HybridCacheEntryOptions()
                        {
                            Expiration = cacheOptions.CarOptionAbsoluteExpiration,
                        },
                        cancellationToken: ct);
                }

                carOptionDtos.Add(carOption);
            }
        }
        else
        {
            var ad = await dbContext.Ads
                .AsNoTracking()
                .Include(a => a.CarOptions)
                .FirstOrDefaultAsync(a => a.Id == query.AdId, ct);

            if (ad is null ||
                (ad.Status is not (AdStatus.PUBLISHED or AdStatus.ARCHIVED or AdStatus.SOLD) &&
                 (!isUserAuthorized || ad.Seller.SellerId != query.UserId!.Value)))
            {
                return Result.Failure<IEnumerable<CarOptionDto>, List<Error>>(Error.NotFound(
                    "ad",
                    $"Ad with id {query.AdId} not found"));
            }

            carOptionDtos =
                ad.CarOptions
                    .Select(o => new CarOptionDto(o.Id, o.OptionType.ToString(), o.Name, o.TechnicalName))
                    .ToList();

            var cacheTasks = carOptionDtos.Select(dto =>
            {
                var carOptionCacheKey = CacheKeyBuilder.Build(
                    nameof(CarOption),
                    dto.Id.ToString());
                return cache.SetAsync(
                    carOptionCacheKey,
                    dto,
                    options: new HybridCacheEntryOptions() { Expiration = cacheOptions.CarOptionAbsoluteExpiration },
                    cancellationToken: ct).AsTask();
            });

            await Task.WhenAll(cacheTasks);

            var idsToCache = ad.CarOptions.Select(o => o.Id).ToList();

            await cache.SetAsync(
                query.CacheKey,
                idsToCache,
                options: new HybridCacheEntryOptions() { Expiration = cacheOptions.CarOptionIndexAbsoluteExpiration },
                cancellationToken: ct);
        }

        return Result.Success<IEnumerable<CarOptionDto>, List<Error>>(carOptionDtos);
    }
}