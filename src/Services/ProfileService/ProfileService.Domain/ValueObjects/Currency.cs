using System.Text.Json.Serialization;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;

namespace ProfileService.Domain.ValueObjects;

public record Currency
{
    private static readonly IReadOnlyDictionary<string, float> _all =
        new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase)
        {
            ["USD"] = 1f, ["BYN"] = 3f, ["RUB"] = 80f, ["EUR"] = 0.85f,
        };

    public string Code { get; private set; } = "USD";

    public static IReadOnlyDictionary<string, float> SupportedCurrencies => _all;

    protected Currency()
    {
    }

    [JsonConstructor]
    private Currency(string code)
    {
        Code = code;
    }


    public static Result<Currency, Error> Of(string code)
    {
        if (!_all.Keys.Contains(code))
        {
            return Result.Failure<Currency, Error>(Error.Domain(
                "currency_code.not.supported",
                $"Currency code {code} is not supported."));
        }

        return Result.Success<Currency, Error>(new Currency(code));
    }

    public float GetConversionFactor(Currency currency)
    {
        if (this == currency) return 1f;

        return _all[currency.Code] / _all[Code];
    }

    public static Result<float, Error> GetConversionFactor(
        string code1,
        string code2)
    {
        if (code1 == code2) return 1f;

        if (!_all.Keys.Contains(code1))
        {
            return Result.Failure<float, Error>(Error.Domain(
                "currency_code.not.supported",
                $"Currency code {code1} is not supported."));
        }

        if (!_all.Keys.Contains(code2))
        {
            return Result.Failure<float, Error>(Error.Domain(
                "currency_code.not.supported",
                $"Currency code {code2} is not supported."));
        }

        return Result.Success<float, Error>(_all[code2] / _all[code1]);
    }
}
