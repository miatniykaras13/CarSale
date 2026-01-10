using AdService.Application.Abstractions.Data;
using AdService.Application.Abstractions.FileStorage;
using AdService.Contracts.Ads.Default;
using AdService.Contracts.Ads.Default.Snapshots;
using AdService.Contracts.Ads.Extended;
using AdService.Contracts.Ads.Extended.Snapshots;
using AdService.Domain.Enums;
using AdService.Domain.ValueObjects;

namespace AdService.Application.Commands.GetPublishedAdById;

public class GetPublishedAdByIdHandler(
    IAppDbContext dbContext,
    IFileStorage fileStorage)
    : ICommandHandler<GetPublishedAdByIdCommand, Result<AdDto, Error>>
{
    public async Task<Result<AdDto, Error>> Handle(GetPublishedAdByIdCommand command, CancellationToken ct)
    {
        var ad = await dbContext.Ads
            .Include(a => a.CarOptions)
            .Include(a => a.Comment)
            .FirstOrDefaultAsync(a => a.Id == command.AdId, ct);

        if (ad is null || ad.Status != AdStatus.PUBLISHED)
            return Result.Failure<AdDto, Error>(Error.NotFound("ad", $"Ad with id {command.AdId} not found"));

        var moneyTask = GetPriceDtos(ad.Price!, ct);
        var imageTask = GetImageUrls(ad.Images, ct);

        await Task.WhenAll(moneyTask, imageTask);

        var moneyResult = await moneyTask;
        var imageResult = await imageTask;

        if (moneyResult.IsFailure) return Result.Failure<AdDto, Error>(moneyResult.Error);
        if (imageResult.IsFailure) return Result.Failure<AdDto, Error>(imageResult.Error);

        var carOptionDtos = ad.CarOptions.Select(carOption =>
                new CarOptionDto(
                    carOption.Id,
                    carOption.OptionType.ToString(),
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
            sellerDto,
            carSnapshotDto,
            imageResult.Value,
            commentDto,
            carOptionDtos);

        ad.IncreaseViews();

        return Result.Success<AdDto, Error>(adDto);
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