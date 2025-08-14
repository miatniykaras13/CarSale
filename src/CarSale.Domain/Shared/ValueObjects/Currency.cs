using CSharpFunctionalExtensions;

namespace CarSale.Domain.Shared.ValueObjects;

public record Currency(string CurrencyCode)
{
    private static readonly ISet<string> _all = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        { "USD", "BYN", "RUB", "EUR" };

    public static List<string> GetSupportedCurrencies() => _all.ToList();

    public static Result<Currency> Of(string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode) || !_all.Contains(currencyCode))
        {
            return Result.Failure<Currency>("Invalid currency");
        }

        return Result.Success(new Currency(currencyCode));
    }
}