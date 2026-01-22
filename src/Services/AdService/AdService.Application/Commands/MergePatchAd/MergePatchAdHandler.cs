using AdService.Application.Abstractions.Data;
using AdService.Application.Abstractions.Helpers;
using AdService.Contracts.Ads.MergePatch;
using AdService.Domain.ValueObjects;

namespace AdService.Application.Commands.MergePatchAd;

public class MergePatchAdCommandHandler(
    IAppDbContext dbContext,
    IMergePatchHelper mergePatchHelper)
    : ICommandHandler<MergePatchAdCommand, UnitResult<List<Error>>>
{
    public async Task<UnitResult<List<Error>>> Handle(
        MergePatchAdCommand command,
        CancellationToken ct)
    {
        var ad = await dbContext.Ads.FindAsync([command.AdId], ct);

        if (ad is null)
            return UnitResult.Failure<List<Error>>(Error.NotFound("ad", $"Ad with id {command.AdId} not found"));

        if (ad.Seller.SellerId != command.UserId)
        {
            return UnitResult.Failure<List<Error>>(Error.Forbidden(
                "ad",
                $"Authenticated user does not own the ad"));
        }

        var adDto = new AdNoCarDto
        {
            Title = ad.Title,
            Description = ad.Description,
            Seller = new SellerSnapshotDto
            {
                DisplayName = ad.Seller.DisplayName,
                Phone = new PhoneNumberDto { Number = ad.Seller.PhoneNumber?.E164 },
            },
            Location = new LocationDto { City = ad.Location?.City, Region = ad.Location?.Region },
            Price = new MoneyDto
            {
                Currency = new CurrencyDto { CurrencyCode = ad.Price?.Currency.CurrencyCode },
                Amount = ad.Price?.Amount,
            },
        };

        var patchedDto = mergePatchHelper.ApplyMergePatch(adDto, command.Patch);

        bool priceChanged = patchedDto.Price?.Amount != ad.Price?.Amount ||
                            patchedDto.Price?.Currency?.CurrencyCode != ad.Price?.Currency.CurrencyCode;
        bool locationChanged = patchedDto.Location?.City != ad.Location?.City ||
                               patchedDto.Location?.Region != ad.Location?.Region;

        Money? price = ad.Price;
        Location? location = ad.Location;

        if (priceChanged)
        {
            var patchedPrice = patchedDto.Price;

            if (patchedPrice is not null &&
                ((patchedPrice.Currency?.CurrencyCode is not null && patchedPrice.Amount is null) ||
                 (patchedPrice.Currency?.CurrencyCode is null && patchedPrice.Amount is not null)))
            {
                return UnitResult.Failure<List<Error>>(Error.Validation(
                    "ad.price",
                    "Both currency and amount must be provided when price is provided."));
            }

            Currency? currency = null;

            if (patchedPrice?.Currency?.CurrencyCode is not null)
            {
                var currencyResult = Currency.Of(patchedPrice.Currency.CurrencyCode);
                if (currencyResult.IsFailure) return UnitResult.Failure<List<Error>>(currencyResult.Error);
                currency = currencyResult.Value;
            }

            if (patchedPrice?.Amount is not null && currency is not null)
            {
                var priceResult = Money.Of(currency, patchedPrice.Amount.Value);
                if (priceResult.IsFailure) return UnitResult.Failure<List<Error>>(priceResult.Error);
                price = priceResult.Value;
            }
        }

        if (locationChanged)
        {
            var patchedLocation = patchedDto.Location;

            if (patchedLocation is { City: not null, Region: null })
            {
                return UnitResult.Failure<List<Error>>(Error.Validation(
                    "ad.location",
                    "City and currency must be both provided when price is not null."));
            }

            if (!(patchedLocation?.City is null && patchedLocation?.Region is null))
            {
                var locationResult = Location.Of(patchedLocation.Region, patchedLocation.City);

                if (locationResult.IsFailure) return UnitResult.Failure<List<Error>>(locationResult.Error);

                location = locationResult.Value;
            }
        }


        var previousSeller = ad.Seller;
        PhoneNumber? phoneNumber = null;

        if (patchedDto.Seller?.Phone?.Number is not null)
        {
            var phoneResult = PhoneNumber.Of(patchedDto.Seller.Phone.Number);
            if (phoneResult.IsFailure) return UnitResult.Failure<List<Error>>(phoneResult.Error);
            phoneNumber = phoneResult.Value;
        }

        var sellerResult = SellerSnapshot.Of(
            previousSeller.SellerId,
            patchedDto.Seller?.DisplayName,
            previousSeller.RegistrationDate,
            previousSeller.ImageId,
            phoneNumber);

        if (sellerResult.IsFailure) return UnitResult.Failure<List<Error>>(sellerResult.Error);


        var updateResult = ad.UpdateMergePatch(
            title: patchedDto.Title,
            description: patchedDto.Description,
            seller: sellerResult.Value,
            location: location,
            price: price);
        if (updateResult.IsFailure) return UnitResult.Failure<List<Error>>(updateResult.Error);

        await dbContext.SaveChangesAsync(ct);

        return UnitResult.Success<List<Error>>();
    }
}