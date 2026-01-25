using AdService.Application.Abstractions.Caching;
using AdService.Application.Abstractions.Data;
using AdService.Application.Abstractions.FileStorage;
using AdService.Contracts.Ads.Default;
using AdService.Contracts.Ads.Default.Snapshots;
using AdService.Contracts.Ads.Extended;
using AdService.Contracts.Ads.Extended.Snapshots;
using AdService.Domain.Aggregates;
using AdService.Domain.Enums;
using AdService.Domain.ValueObjects;

namespace AdService.Application.Queries.GetAdById;

public class GetAdByIdCommandHandler(
    IAppDbContext dbContext,
    IFileStorage fileStorage,
    ICacheService cacheService)
    : ICommandHandler<GetAdByIdQuery, Result<AdDto, List<Error>>>
{
    public async Task<Result<AdDto, List<Error>>> Handle(GetAdByIdQuery query, CancellationToken ct)
    {
        var userAuthorized = query.UserId is not null;

        Ad? ad;

        ad = await cacheService.GetDataAsync<Ad>($"ad_{query.AdId}", ct)
             ?? await dbContext.Ads
            .Include(a => a.CarOptions)
            .Include(a => a.Comment)
            .FirstOrDefaultAsync(a => a.Id == query.AdId, ct);

        if (ad is null ||
            (ad.Status is not (AdStatus.PUBLISHED or AdStatus.ARCHIVED) &&
             (!userAuthorized || (userAuthorized && ad.Seller.SellerId != query.UserId!.Value))))
            return Result.Failure<AdDto, List<Error>>(Error.NotFound("ad", $"Ad with id {query.AdId} not found"));
        
        

        var moneyTask = GetPriceDtos(ad.Price!, ct);
        var imageTask = GetImageUrls(ad.Images, ct);

        await Task.WhenAll(moneyTask, imageTask);

        var moneyResult = await moneyTask;
        var imageResult = await imageTask;

        if (moneyResult.IsFailure)
            return Result.Failure<AdDto, List<Error>>(moneyResult.Error);
        if (imageResult.IsFailure)
            return Result.Failure<AdDto, List<Error>>(imageResult.Error);

        var carOptionDtos = ad.CarOptions.Select(carOption =>
                new CarOptionDto(
                    carOption.Id,
                    carOption.OptionType,
                    carOption.Name,
                    carOption.TechnicalName))
            .ToList();

        var phoneNumberDto = new PhoneNumberDto(ad.Seller.PhoneNumber!.E164);

        var sellerDto = new SellerSnapshotDto(ad.Seller.SellerId, ad.Seller.DisplayName, phoneNumberDto);

        var adLocation = ad.Location!;
        var locationDto = new LocationDto(adLocation.Region!, adLocation.City!);

        var adsCar = ad.Car!;

        var carSnapshotDto = new CarSnapshotDto(
            adsCar.CarId!.Value,
            new BrandSnapshotDto(
                adsCar.Brand!.Id,
                adsCar.Brand.Name),
            new ModelSnapshotDto(
                adsCar.Model!.Id,
                adsCar.Model.Name),
            new GenerationSnapshotDto(
                adsCar.Generation!.Id,
                adsCar.Generation.Name),
            new EngineSnapshotDto(
                adsCar.Engine!.Id,
                adsCar.Engine.Name,
                new FuelTypeSnapshotDto(adsCar.Engine.FuelType.Id, adsCar.Engine.FuelType.Name),
                adsCar.Engine.HorsePower),
            new AutoDriveTypeSnapshotDto(
                adsCar.DriveType!.Id,
                adsCar.DriveType.Name),
            new TransmissionTypeSnapshotDto(
                adsCar.TransmissionType!.Id,
                adsCar.TransmissionType.Name),
            new BodyTypeSnapshotDto(
                adsCar.BodyType!.Id,
                adsCar.BodyType.Name),
            adsCar.Year!.Value,
            adsCar.Consumption,
            adsCar.Vin,
            adsCar.Mileage,
            adsCar.Color);


        CommentDto? commentDto = null;
        if (ad.Comment is not null)
            commentDto = new CommentDto(ad.Comment.Message, ad.Comment.CreatedAt);

        var adDto = new AdDto(
            ad.Id,
            ad.Title!,
            ad.Description,
            moneyResult.Value,
            locationDto,
            ad.Views,
            ad.Status,
            sellerDto,
            carSnapshotDto,
            imageResult.Value,
            commentDto,
            carOptionDtos);

        // ad.IncreaseViews(); // todo сделать отдельный эндпоинт post ads/{id}/views
        return Result.Success<AdDto, List<Error>>(adDto);
    }

    private async Task<Result<List<MoneyDto>, Error>> GetPriceDtos(Money price, CancellationToken ct)
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