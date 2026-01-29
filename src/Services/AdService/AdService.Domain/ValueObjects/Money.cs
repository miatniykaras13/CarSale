using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace AdService.Domain.ValueObjects;

public record Money
{
    public Currency Currency { get; private set; }

    public int Amount { get; private set; }

    protected Money()
    {
    }

    [JsonConstructor]
    private Money(Currency currency, int amount)
    {
        Currency = currency;
        Amount = amount;
    }

    public static Result<Money, Error> Of(Currency currency, int amount)
    {
        Money money = new(currency, amount);
        return Result.Success<Money, Error>(money);
    }
}