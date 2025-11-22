using CSharpFunctionalExtensions;

namespace AdService.Domain.ValueObjects;

public record Money
{
    public Currency Currency { get; private set; } = null!;

    public int Amount { get; private set; }

    public const int MIN_AMOUNT = 100;

    protected Money()
    {
    }

    private Money(Currency currency, int amount)
    {
        Currency = currency;
        Amount = amount;
    }

    public static Result<Money, Error> Of(Currency currency, int amount)
    {
        if (amount <= MIN_AMOUNT)
        {
            return Result.Failure<Money, Error>(Error.Domain(
                "money.amount.too_low",
                $"Amount must be greater than {MIN_AMOUNT}"));
        }

        Money money = new(currency, amount);
        return Result.Success<Money, Error>(money);
    }
}