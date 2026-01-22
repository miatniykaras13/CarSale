using CSharpFunctionalExtensions;

namespace AdService.Domain.ValueObjects;

public record Currency
{
    private static readonly IReadOnlyDictionary<string, float> _all =
        new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase)
        {
            ["USD"] = 1f, ["BYN"] = 3f, ["RUB"] = 80f, ["EUR"] = 0.85f,
        };

    public string CurrencyCode { get; private set; } = "USD";

    public static IReadOnlyDictionary<string, float> SupportedCurrencies => _all;

    protected Currency()
    {
    }

    private Currency(string currencyCode)
    {
        CurrencyCode = currencyCode;
    }


    public static Result<Currency, Error> Of(string currencyCode)
    {
        if (!_all.Keys.Contains(currencyCode))
        {
            return Result.Failure<Currency, Error>(Error.Domain(
                "currency_code.not.supported",
                $"Currency code {currencyCode} is not supported."));
        }

        return Result.Success<Currency, Error>(new Currency(currencyCode));
    }

    public float GetConversionFactor(Currency currency) // возвращает на что нужно умножить, чтобы получить новую валюту
    {
        if (this == currency) return 1f;

        return _all[currency.CurrencyCode] / _all[CurrencyCode];
    }

    public static Result<float, Error> GetConversionFactor(
        string currencyCode1,
        string currencyCode2) // возвращает на что нужно умножить, чтобы получить новую валюту
    {
        if (currencyCode1 == currencyCode2) return 1f;

        if (!_all.Keys.Contains(currencyCode1))
        {
            return Result.Failure<float, Error>(Error.Domain(
                "currency_code.not.supported",
                $"Currency code {currencyCode1} is not supported."));
        }

        if (!_all.Keys.Contains(currencyCode2))
        {
            return Result.Failure<float, Error>(Error.Domain(
                "currency_code.not.supported",
                $"Currency code {currencyCode2} is not supported."));
        }

        return Result.Success<float, Error>(_all[currencyCode2] / _all[currencyCode1]);
    }
}