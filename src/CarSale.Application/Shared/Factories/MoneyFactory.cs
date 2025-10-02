using CarSale.Contracts.Shared;
using CarSale.Domain.Shared.ValueObjects;
using CSharpFunctionalExtensions;

namespace CarSale.Application.Shared.Factories;

public class MoneyFactory
{
    public static Result<Money> FromDto(MoneyDto moneyDto)
    {
        Result<Currency> currencyResult = Currency.Of(moneyDto.CurrencyDto.CurrencyCode);

        if (currencyResult.IsFailure)
        {
            return Result.Failure<Money>(currencyResult.Error);
        }

        Result<Money> moneyResult = Money.Of(currencyResult.Value, moneyDto.Amount);
        if (moneyResult.IsFailure)
        {
            return Result.Failure<Money>(moneyResult.Error);
        }

        return Result.Success(moneyResult.Value);
    }
}