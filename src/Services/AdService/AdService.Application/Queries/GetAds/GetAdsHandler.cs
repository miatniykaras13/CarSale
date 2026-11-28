using AdService.Application.Abstractions.Data;
using AdService.Application.Abstractions.FileStorage;
using AdService.Contracts.Ads.Default;
using AdService.Contracts.Ads.Default.Snapshots;
using AdService.Contracts.Ads.ListItems;
using AdService.Domain.Aggregates;
using AdService.Domain.Enums;
using AdService.Domain.ValueObjects;

namespace AdService.Application.Queries.GetAds;

public class GetAdsHandler(IAppDbContext dbContext, IFileStorage fileStorage)
    : IQueryHandler<GetAdsQuery, Result<AdsPageDto, List<Error>>>
{
    public async Task<Result<AdsPageDto, List<Error>>> Handle(GetAdsQuery query, CancellationToken ct)
    {
        var (pageNumber, pageSize) = query.PageParameters;
        var offset = checked((pageNumber - 1) * pageSize);
        var userId = query.UserId;

        var visibleAds = dbContext.Ads.AsNoTracking()
            .Filter(query.Filter)
            .Where(ad => ad.Status == AdStatus.PUBLISHED ||
                         ad.Status == AdStatus.ARCHIVED ||
                         ad.Status == AdStatus.SOLD ||
                         (userId != null && ad.Seller.SellerId == userId));

        // Apply the offset to IDs so only the selected ads need joins for their card data.
        var pageIds = OrderAds(visibleAds, query.Sort)
            .Skip(offset)
            .Take(pageSize)
            .Select(ad => ad.Id);
        var pageAds = dbContext.Ads.AsNoTracking()
            .Join(pageIds, ad => ad.Id, id => id, (ad, _) => ad);
        var pageQuery = ProjectListItems(OrderAds(pageAds, query.Sort));

        var (totalCount, ads) = await dbContext.ReadSnapshotAsync(async cancellationToken =>
        {
            var count = await visibleAds.CountAsync(cancellationToken);
            var items = await pageQuery.ToListAsync(cancellationToken);
            return (count, items);
        }, ct);

        List<AdListItemDto> items = new(ads.Count);
        foreach (var ad in ads)
        {
            List<MoneyDto> prices = [];
            if (ad.PriceAmount is int amount)
            {
                var priceResult = GetPriceDtos(amount, ad.CurrencyCode!);
                if (priceResult.IsFailure)
                    return Result.Failure<AdsPageDto, List<Error>>(priceResult.Error);

                prices = priceResult.Value;
            }

            items.Add(new AdListItemDto(ad.Id, ad.Title, ad.Description, ad.Status.ToString(), ad.Car,
                ad.Seller, ad.ImageId, prices, ad.Location));
        }

        if (query.IncludeImageUrls)
        {
            await Task.WhenAll(items.Where(item => item.ImageId.HasValue).Select(async item =>
            {
                item.ImageUrl = await fileStorage.GetDownloadLinkAsync(item.ImageId!.Value, 600, ct);
            }));
        }

        return Result.Success<AdsPageDto, List<Error>>(new AdsPageDto(items, pageNumber, pageSize, totalCount));
    }

    private static IOrderedQueryable<Ad> OrderAds(IQueryable<Ad> ads, string sort) =>
        sort == GetAdsQuery.Oldest
            ? ads.OrderBy(ad => ad.CreatedAt).ThenBy(ad => ad.Id)
            : ads.OrderByDescending(ad => ad.CreatedAt).ThenByDescending(ad => ad.Id);

    private static IQueryable<AdListItemRow> ProjectListItems(IQueryable<Ad> ads) =>
        ads.Select(ad => new AdListItemRow(
            ad.Id,
            ad.Title,
            ad.Description,
            ad.Status,
            new CarSnapshotListItemDto(
                ad.Car!.CarId,
                ad.Car.Brand!.Name,
                ad.Car.Model!.Name,
                ad.Car.Generation!.Name,
                ad.Car.Year,
                ad.Car.DriveType!.Name,
                ad.Car.TransmissionType!.Name,
                (float?)ad.Car.Engine!.Volume,
                ad.Car.Engine.FuelType.Name,
                ad.Car.BodyType!.Name),
            new SellerSnapshotDto(ad.Seller.SellerId, ad.Seller.DisplayName,
                ad.Seller.PhoneNumber == null ? null : new PhoneNumberDto(ad.Seller.PhoneNumber.E164)),
            ad.Images.OrderBy(image => image.Id).Select(image => (Guid?)image.Id).FirstOrDefault(),
            (int?)ad.Price!.Amount,
            ad.Price.Currency.CurrencyCode,
            ad.Location == null ? null : new LocationDto(ad.Location.City, ad.Location.Region)));

    private static Result<List<MoneyDto>, Error> GetPriceDtos(int amount, string currencyCode)
    {
        List<MoneyDto> prices = [];
        foreach (var currency in Currency.SupportedCurrencies)
        {
            var conversionFactor = Currency.GetConversionFactor(currencyCode, currency.Key);
            if (conversionFactor.IsFailure)
                return Result.Failure<List<MoneyDto>, Error>(conversionFactor.Error);

            prices.Add(new MoneyDto((int)(amount * conversionFactor.Value), new CurrencyDto(currency.Key)));
        }

        return Result.Success<List<MoneyDto>, Error>(prices);
    }

    private sealed record AdListItemRow(
        Guid Id,
        string? Title,
        string? Description,
        AdStatus Status,
        CarSnapshotListItemDto Car,
        SellerSnapshotDto Seller,
        Guid? ImageId,
        int? PriceAmount,
        string? CurrencyCode,
        LocationDto? Location);
}
