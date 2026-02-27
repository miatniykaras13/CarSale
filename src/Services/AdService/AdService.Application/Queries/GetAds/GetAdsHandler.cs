using AdService.Application.Abstractions.Data;
using AdService.Application.Abstractions.FileStorage;
using AdService.Application.Builders;
using AdService.Contracts.Ads.Default;
using AdService.Contracts.Ads.Default.Snapshots;
using AdService.Contracts.Ads.ListItems;
using AdService.Domain.Aggregates;
using AdService.Domain.Enums;
using AdService.Domain.ValueObjects;
using Microsoft.Extensions.Caching.Hybrid;
using CurrencyDto = AdService.Contracts.Ads.Default.CurrencyDto;
using LocationDto = AdService.Contracts.Ads.Default.LocationDto;
using MoneyDto = AdService.Contracts.Ads.Default.MoneyDto;

namespace AdService.Application.Queries.GetAds;

public class GetAdByIdQueryHandler(
    IAppDbContext dbContext,
    IFileStorage fileStorage,
    HybridCache cache)
    : IQueryHandler<GetAdsQuery, Result<List<AdListItemDto>, List<Error>>>
{
    public async Task<Result<List<AdListItemDto>, List<Error>>> Handle(GetAdsQuery query, CancellationToken ct)
    {
        // хранение ids по фильтрам
        // хранение dtos по id
        var page = query.PageParameters.PageNumber;
        var pageSize = query.PageParameters.PageSize;

        List<AdListItemDto> adDtos = [];

        var ids = await cache.GetOrCreateAsync<List<Guid>?>(
            query.CacheKey,
            factory: null!,
            options: new HybridCacheEntryOptions() { Flags = HybridCacheEntryFlags.DisableUnderlyingData },
            cancellationToken: ct);

        if (ids is not null)
        {
            var taskList = ids.Select(id =>
                cache.GetOrCreateAsync<AdListItemDto?>(
                    CacheKeyBuilder.BuildListItem(nameof(Ad), id.ToString()),
                    factory: null!,
                    options: new HybridCacheEntryOptions() { Flags = HybridCacheEntryFlags.DisableUnderlyingData },
                    cancellationToken: ct).AsTask()).ToArray();

            await Task.WhenAll(taskList);

            for (int i = 0; i < taskList.Length; i++)
            {
                var adListItemDto = await taskList[i];
                if (adListItemDto is not null)
                {
                    adDtos.Add(adListItemDto);
                }
                else
                {
                    var ad = await dbContext.Ads.FindAsync([ids[i]], ct);
                    if (ad is null)
                        continue;

                    var adListItemDtoResult = await GetAndCacheListItemDto(ad, query.IncludeImageUrls, ct);
                    if (adListItemDtoResult.IsFailure)
                        return Result.Failure<List<AdListItemDto>, List<Error>>(adListItemDtoResult.Error);
                    adDtos.Add(adListItemDtoResult.Value);
                }
            }
        }
        else
        {
            var ads = await dbContext.Ads
                .Filter(query.Filter)
                .ToListAsync(ct);

            foreach (var ad in ads)
            {
                var adListItemDtoResult = await GetAndCacheListItemDto(ad, query.IncludeImageUrls, ct);
                if (adListItemDtoResult.IsFailure)
                    return Result.Failure<List<AdListItemDto>, List<Error>>(adListItemDtoResult.Error);
                adDtos.Add(adListItemDtoResult.Value);
            }
        }

        var userAuthorized = query.UserId is not null;

        var adIds = adDtos.Select(ad => ad.AdId).ToList();

        await cache.SetAsync(
            CacheKeyBuilder.BuildIndex(nameof(Ad), query.Filter.GetHashCode().ToString()),
            adIds,
            cancellationToken: ct);

        adDtos = adDtos
            .Where(adDto =>
                !(Enum.Parse<AdStatus>(adDto.Status) is not (AdStatus.PUBLISHED or AdStatus.ARCHIVED
                      or AdStatus.SOLD) &&
                  (!userAuthorized || adDto.Seller.SellerId != query.UserId!.Value)))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        if (query.IncludeImageUrls)
        {
            var imageTasks = adDtos.Select(dto =>
                    dto.ImageId is not null ? fileStorage.GetDownloadLinkAsync(dto.ImageId.Value, 600, ct) : null)
                .ToArray();

            await Task.WhenAll(imageTasks.Where(task => task is not null)!);

            for (int i = 0; i < adDtos.Count; i++)
            {
                adDtos[i].ImageUrl = imageTasks[i] is null ? null : await imageTasks[i]!;
            }
        }

        return Result.Success<List<AdListItemDto>, List<Error>>(adDtos);
    }

    private Result<List<MoneyDto>, Error> GetPriceDtos(Money price)
    {
        List<MoneyDto> priceInAllCurrenciesDtos = [];

        var currencies = Currency.SupportedCurrencies;

        foreach (var currency in currencies)
        {
            var currencyDto = new CurrencyDto(currency.Key);

            var conversionFactorResult = Currency.GetConversionFactor(price.Currency.CurrencyCode, currency.Key);

            if (conversionFactorResult.IsFailure)
                return Result.Failure<List<MoneyDto>, Error>(conversionFactorResult.Error);

            var conversionFactor = conversionFactorResult.Value;

            var newAmount = price.Amount * conversionFactor;

            var moneyDto = new MoneyDto((int)newAmount, currencyDto);
            priceInAllCurrenciesDtos.Add(moneyDto);
        }

        return Result.Success<List<MoneyDto>, Error>(priceInAllCurrenciesDtos);
    }

    private async Task<Result<AdListItemDto, Error>> GetAndCacheListItemDto(
        Ad ad,
        bool includeImageUrls,
        CancellationToken ct)
    {
        var adsCar = ad.Car;

        PhoneNumberDto? phoneNumberDto = null;
        if (ad.Seller.PhoneNumber is not null)
            phoneNumberDto = new PhoneNumberDto(ad.Seller.PhoneNumber.E164);

        var sellerDto = new SellerSnapshotDto(
            ad.Seller.SellerId,
            ad.Seller.DisplayName,
            phoneNumberDto);

        var carDto = new CarSnapshotListItemDto(
            adsCar?.CarId,
            adsCar?.Brand?.Name,
            adsCar?.Model?.Name,
            adsCar?.Generation?.Name,
            adsCar?.Year,
            adsCar?.DriveType?.Name,
            adsCar?.TransmissionType?.Name,
            adsCar?.Engine?.Volume,
            adsCar?.Engine?.FuelType.Name,
            adsCar?.BodyType?.Name);

        List<MoneyDto> moneyDtos = [];
        if (ad.Price is not null)
        {
            var moneyDtosResult = GetPriceDtos(ad.Price);
            if (moneyDtosResult.IsFailure)
                return Result.Failure<AdListItemDto, Error>(moneyDtosResult.Error);
            moneyDtos = moneyDtosResult.Value;
        }

        LocationDto? locationDto = null;
        if (ad.Location is not null)
            locationDto = new LocationDto(ad.Location.City, ad.Location.Region);

        Guid? imageId = null;
        string? imageUrl = null;
        if (ad.Images.Any())
            imageId = ad.Images[0];

        var adDto = new AdListItemDto(
            ad.Id,
            ad.Title,
            ad.Description,
            ad.Status.ToString(),
            carDto,
            sellerDto,
            imageId,
            moneyDtos,
            locationDto);

        var adCacheKey = CacheKeyBuilder.BuildListItem(nameof(Ad), ad.Id.ToString());
        await cache.SetAsync(
            adCacheKey,
            adDto,
            cancellationToken: ct);

        adDto.ImageUrl = imageUrl;

        return Result.Success<AdListItemDto, Error>(adDto);
    }
}