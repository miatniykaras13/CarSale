using CSharpFunctionalExtensions;

namespace AdService.Domain.ValueObjects;

public record CountryCode
{
    public string Code { get; private set; } = null!;

    private static readonly Dictionary<string, string> _all = new()
    {
        ["BY"] = "375",
        ["RU"] = "7",
        ["48"] = "48",
    };

    protected CountryCode()
    {
    }

    private CountryCode(string code)
    {
        Code = code;
    }

    public static List<string> GetSupportedCountryCodes => _all.Keys.ToList();

    public static Result<CountryCode, Error> Of(string code)
    {
        if (!_all.Keys.Any(c => c.StartsWith(code)))
        {
            return Result.Failure<CountryCode, Error>(Error.Domain("country_code.not.supported", "Country code is not supported."));
        }

        CountryCode countryCode = new(code);

        return Result.Success<CountryCode, Error>(countryCode);
    }
}