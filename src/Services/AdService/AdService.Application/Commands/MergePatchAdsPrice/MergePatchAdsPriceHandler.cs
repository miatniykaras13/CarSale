using AdService.Application.Abstractions.Data;
using AdService.Application.Commands.MergePatchAdsPrice;
using AdService.Domain.ValueObjects;

namespace AdService.Application.Commands.MergePatchAdsPrice;

public class MergePatchAdsPriceCommandHandler(IAppDbContext dbContext)
    : ICommandHandler<MergePatchAdsPriceCommand, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(
        MergePatchAdsPriceCommand command,
        CancellationToken cancellationToken)
    {
        var ad = await dbContext.Ads.FindAsync([command.AdId], cancellationToken);

        if (ad is null)
            return UnitResult.Failure(Error.NotFound("ad", $"Ad with id {command.AdId} not found"));

        var currentPrice = ad.Price;

        var priceDto = command.PriceDto;

        var currencyDto = priceDto.CurrencyDto;

        var newCurrencyResult = Currency.Of(currencyDto?.CurrencyCode ?? currentPrice?.Currency?.CurrencyCode);

        if (newCurrencyResult.IsFailure) return newCurrencyResult;

        var newPriceResult = Money.Of(
            amount: priceDto.Amount ?? currentPrice?.Amount,
            currency: newCurrencyResult.Value ?? currentPrice?.Currency);

        if (newPriceResult.IsFailure) return newPriceResult;

        var updateResult = ad.Update(price: newPriceResult.Value);

        if (updateResult.IsFailure) return updateResult;

        await dbContext.SaveChangesAsync(cancellationToken);

        return UnitResult.Success<Error>();
    }
}