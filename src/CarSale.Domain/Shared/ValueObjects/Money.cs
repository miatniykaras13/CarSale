using CSharpFunctionalExtensions;

namespace CarSale.Domain.Shared.ValueObjects;

public record Money(Currency Currency, decimal Amount)
{
    public static Result<Money> Of(Currency currency, decimal amount)
    {
        if (amount <= 0)
        {
            return Result.Failure<Money>("Amount must be greater than zero");
        }

        var money = new Money(currency, amount);
        return Result.Success(money);
    }
}