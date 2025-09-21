using CSharpFunctionalExtensions;

namespace CarSale.Domain.Shared.ValueObjects
{
    public record CountryCode(string Code)
    {
        private static readonly Dictionary<string, string> _all = new()
        {
            { "375", "BY" },
            { "7",   "RU" },
            { "380", "UA" },
            { "48",  "PL" },
        };

        public static List<string> GetSupportedCountryCodes => _all.Keys.ToList();

        public static Result<CountryCode> Of(string code)
        {
            if (string.IsNullOrWhiteSpace(code) || !_all.Keys.Any(c => c.StartsWith(code)))
            {
                return Result.Failure<CountryCode>("Invalid country code");
            }

            var countryCode = new CountryCode(code);

            return Result.Success(countryCode);
        }
    }
}