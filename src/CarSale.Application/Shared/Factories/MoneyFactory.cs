using CarSale.Contracts.Shared;
using CarSale.Domain.Shared.ValueObjects;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSale.Application.Shared.Factories
{
    public class MoneyFactory
    {
        public static Result<Money> FromDto(MoneyDto moneyDto)
        {
            var currencyResult = Currency.Of(moneyDto.CurrencyDto.CurrencyCode);

            if (currencyResult.IsFailure)
            {
                return Result.Failure<Money>(currencyResult.Error);
            }

            var moneyResult = Money.Of(currencyResult.Value, moneyDto.Amount);
            if (moneyResult.IsFailure)
            {
                return Result.Failure<Money>(moneyResult.Error);
            }

            return Result.Success(moneyResult.Value);
        }
    }
}
