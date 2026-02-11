using AdService.Application.Abstractions.Data;
using AdService.Application.Abstractions.FileStorage;
using AdService.Contracts.Ads.Default;
using AdService.Contracts.Ads.Default.Snapshots;
using AdService.Contracts.Ads.Extended;
using AdService.Contracts.Ads.Extended.Snapshots;
using AdService.Domain.Enums;
using AdService.Domain.ValueObjects;
using Microsoft.Extensions.Caching.Hybrid;

namespace AdService.Application.Queries.GetAdById;

public class GetAdByIdQueryHandler(
    IAppDbContext dbContext,
    IFileStorage fileStorage,
    HybridCache cache)
    : IQueryHandler<GetAdByIdQuery, Result<AdDto, List<Error>>>
{
    public async Task<Result<AdDto, List<Error>>> Handle(GetAdByIdQuery query, CancellationToken ct)
    {
        var userAuthorized = query.UserId is not null;

        var adDto = await cache.GetOrCreateAsync<AdDto?>(
            query.CacheKey,
            factory: null!,
            options: new HybridCacheEntryOptions() { Flags = HybridCacheEntryFlags.DisableUnderlyingData },
            cancellationToken: ct);

        if (adDto is not null) return adDto;

        var ad = await dbContext.Ads
            .AsNoTracking()
            .Include(a => a.CarOptions)
            .Include(a => a.Comment)
            .FirstOrDefaultAsync(a => a.Id == query.AdId, ct);

        if (ad is null ||
            (ad.Status is not (AdStatus.PUBLISHED or AdStatus.ARCHIVED) &&
             (!userAuthorized || (userAuthorized && ad.Seller.SellerId != query.UserId!.Value))))
            return Result.Failure<AdDto, List<Error>>(Error.NotFound("ad", $"Ad with id {query.AdId} not found"));

        List<MoneyDto> moneyDtos = [];

        if (ad.Price is not null)
        {
            var moneyResult = GetPriceDtos(ad.Price);
            if (moneyResult.IsFailure)
                return Result.Failure<AdDto, List<Error>>(moneyResult.Error);
            moneyDtos = moneyResult.Value;
        }

        var imageResult = await GetImageUrls(ad.Images, ct);

        if (imageResult.IsFailure)
            return Result.Failure<AdDto, List<Error>>(imageResult.Error);

        var carOptionDtos = ad.CarOptions.Select(carOption =>
                new CarOptionDto(
                    carOption.Id,
                    carOption.OptionType,
                    carOption.Name,
                    carOption.TechnicalName))
            .ToList();

        PhoneNumberDto? phoneNumberDto = null;
        if (ad.Seller.PhoneNumber is not null)
            phoneNumberDto = new PhoneNumberDto(ad.Seller.PhoneNumber!.E164);

        var sellerDto = new SellerSnapshotDto(ad.Seller.SellerId, ad.Seller.DisplayName, phoneNumberDto);

        LocationDto? locationDto = null;
        if (ad.Location is not null)
            locationDto = new LocationDto(ad.Location.Region, ad.Location.City!);

        var adsCar = ad.Car;

        CarSnapshotDto? carSnapshotDto = null;

        if (adsCar is not null)
        {
            carSnapshotDto = new CarSnapshotDto(
                adsCar.CarId,
                adsCar.Brand is not null ? new BrandSnapshotDto(adsCar.Brand.Id, adsCar.Brand.Name) : null,
                adsCar.Model is not null ? new ModelSnapshotDto(adsCar.Model.Id, adsCar.Model.Name) : null,
                adsCar.Generation is not null
                    ? new GenerationSnapshotDto(adsCar.Generation.Id, adsCar.Generation.Name)
                    : null,
                adsCar.Engine is not null
                    ? new EngineSnapshotDto(adsCar.Engine.Id, adsCar.Engine.Name,
                        new FuelTypeSnapshotDto(adsCar.Engine.FuelType.Id, adsCar.Engine.FuelType.Name),
                        adsCar.Engine.HorsePower)
                    : null,
                adsCar.DriveType is not null
                    ? new AutoDriveTypeSnapshotDto(adsCar.DriveType.Id, adsCar.DriveType.Name)
                    : null,
                adsCar.TransmissionType is not null
                    ? new TransmissionTypeSnapshotDto(adsCar.TransmissionType.Id, adsCar.TransmissionType.Name)
                    : null,
                adsCar.BodyType is not null ? new BodyTypeSnapshotDto(adsCar.BodyType.Id, adsCar.BodyType.Name) : null,
                adsCar.Year,
                adsCar.Consumption,
                adsCar.Vin,
                adsCar.Mileage,
                adsCar.Color);
        }


        CommentDto? commentDto = null;
        if (ad.Comment is not null)
            commentDto = new CommentDto(ad.Comment.Message, ad.Comment.CreatedAt);

        adDto = new AdDto(
            ad.Id,
            ad.Title,
            ad.Description,
            moneyDtos,
            locationDto,
            ad.Views,
            ad.Status,
            sellerDto,
            carSnapshotDto,
            imageResult.Value,
            commentDto,
            carOptionDtos);

        await cache.SetAsync(query.CacheKey, adDto, cancellationToken: ct);

        // ad.IncreaseViews(); // todo сделать отдельный эндпоинт post ads/{id}/views
        return adDto;
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

    private async Task<Result<List<string>, Error>> GetImageUrls(IReadOnlyList<Guid> imageIds, CancellationToken ct)
    {
        List<string> imageUrls = [];

        foreach (var imageId in imageIds)
        {
            var imageUrl = await fileStorage.GetDownloadLinkAsync(imageId, 600, ct);
            imageUrls.Add(imageUrl);
        }

        return Result.Success<List<string>, Error>(imageUrls);
    }
}