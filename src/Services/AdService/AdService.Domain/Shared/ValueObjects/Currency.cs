﻿using CSharpFunctionalExtensions;

namespace AdService.Domain.Shared.ValueObjects;

public record Currency
{
    public string CurrencyCode { get; private set; } = null!;

    private static readonly IReadOnlyDictionary<string, float> _all = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase)
    {
        ["USD"] = 1f,
        ["BYN"] = 3f,
        ["RUB"] = 80f,
        ["EUR"] = 0.85f,
    };

    protected Currency()
    {
    }

    private Currency(string currencyCode)
    {
        CurrencyCode = currencyCode;
    }

    public static List<string> GetSupportedCurrencies() => _all.Keys.ToList();

    public static Result<Currency, Error> Of(string currencyCode)
    {
        if (!_all.Keys.Contains(currencyCode))
        {
            return Result.Failure<Currency, Error>(Error.Domain("currency_code.not.supported", "Currency code is not supported."));
        }

        return Result.Success<Currency, Error>(new Currency(currencyCode));
    }
}