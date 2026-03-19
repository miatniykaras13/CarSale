using AdService.Application.Abstractions.Data;
using AdService.Application.Builders;
using AdService.Application.Options;
using AdService.Contracts.Ads.Default;
using AdService.Domain.Entities;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;

namespace AdService.Application.Queries.GetAllCarOptions;

public class GetAllCarOptionsQueryHandler(
    IAppDbContext dbContext,
    HybridCache cache,
    IOptions<CacheOptions> options)
    : IQueryHandler<GetAllCarOptionsQuery, Result<IEnumerable<CarOptionDto>, List<Error>>>
{
    private readonly CacheOptions _cacheOptions = options.Value;

    public async Task<Result<IEnumerable<CarOptionDto>, List<Error>>> Handle(
        GetAllCarOptionsQuery query,
        CancellationToken ct)
    {
        List<CarOptionDto> carOptionDtos = [];

        var ids = await cache.GetOrCreateAsync<List<int>?>(
            query.CacheKey,
            factory: null!,
            options: new HybridCacheEntryOptions() { Flags = HybridCacheEntryFlags.DisableUnderlyingData },
            cancellationToken: ct);

        if (ids is not null)
        {
            var taskList = ids.Select(id =>
                cache.GetOrCreateAsync<CarOptionDto?>(
                    CacheKeyBuilder.Build(nameof(CarOption), id.ToString()),
                    factory: null!,
                    options: new HybridCacheEntryOptions() { Flags = HybridCacheEntryFlags.DisableUnderlyingData },
                    cancellationToken: ct).AsTask()).ToArray();

            await Task.WhenAll(taskList);

            for (int i = 0; i < taskList.Length; i++)
            {
                var carOptionDto = await taskList[i];
                if (carOptionDto is null)
                {
                    var carOptionCacheKey = CacheKeyBuilder.Build(nameof(CarOption), ids[i].ToString());

                    var carOption = await dbContext.CarOptions.FindAsync([ids[i]], ct);
                    if (carOption is null)
                    {
                        await cache.RemoveAsync(
                            carOptionCacheKey,
                            cancellationToken: ct);
                        continue;
                    }

                    carOptionDto = new CarOptionDto(
                        carOption.Id,
                        carOption.OptionType.ToString(),
                        carOption.Name,
                        carOption.TechnicalName);

                    await cache.SetAsync(
                        carOptionCacheKey,
                        carOptionDto,
                        new HybridCacheEntryOptions() { Expiration = _cacheOptions.CarOptionAbsoluteExpiration },
                        cancellationToken: ct);
                }

                carOptionDtos.Add(carOptionDto);
            }
        }
        else
        {
            var carOptionsFromDb = await dbContext.CarOptions
                .AsNoTracking()
                .Filter(query.Filter)
                .OrderBy(o => o.Id)
                .Select(o => new CarOptionDto(o.Id, o.OptionType.ToString(), o.Name, o.TechnicalName))
                .ToListAsync(ct);

            var cacheTasks = carOptionsFromDb.Select(carOptionDto =>
                cache.SetAsync(
                    CacheKeyBuilder.Build(nameof(CarOption), carOptionDto.Id.ToString()),
                    carOptionDto,
                    new HybridCacheEntryOptions() { Expiration = _cacheOptions.CarOptionAbsoluteExpiration },
                    cancellationToken: ct).AsTask()).ToArray();

            await Task.WhenAll(cacheTasks);

            carOptionDtos.AddRange(carOptionsFromDb);

            ids = carOptionDtos.Select(o => o.Id).ToList();

            await cache.SetAsync(
                query.CacheKey,
                ids,
                new HybridCacheEntryOptions() { Expiration = _cacheOptions.CarOptionIndexAbsoluteExpiration },
                cancellationToken: ct);
        }

        var pagedCarOptionDtos = carOptionDtos
            .Skip((query.PageParameters.PageNumber - 1) * query.PageParameters.PageSize)
            .Take(query.PageParameters.PageSize);

        return Result.Success<IEnumerable<CarOptionDto>, List<Error>>(pagedCarOptionDtos);
    }
}