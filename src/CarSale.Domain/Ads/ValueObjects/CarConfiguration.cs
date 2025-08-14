using CarSale.Domain.Ads.Enums;
using CSharpFunctionalExtensions;

namespace CarSale.Domain.Ads.ValueObjects;

public record CarConfiguration(
    InteriorType Interior,
    SafetyOptions Safety,
    ComfortOptions Comfort,
    AssistanceSystems Assistance,
    OpticsType Optics
)
{
    public static Result<CarConfiguration> Of(
        string interior,
        List<string> safety,
        List<string> comfort,
        List<string> assistance,
        string optics)
    {
        var interiorType = Enum.Parse<InteriorType>(interior);
        var opticsType = Enum.Parse<OpticsType>(optics);
        var safetyOptionsResult = ParseFlags<SafetyOptions>(safety);
        var comfortOptionsResult = ParseFlags<ComfortOptions>(comfort);
        var assistanceSystemsResult = ParseFlags<AssistanceSystems>(assistance);


        return Result.Success(
            new CarConfiguration(
                interiorType,
                safetyOptionsResult.Value,
                comfortOptionsResult.Value,
                assistanceSystemsResult.Value,
                opticsType));
    }

    private static Result<TEnum> ParseFlags<TEnum>(IEnumerable<string> values) where TEnum : struct, Enum
    {
        IEnumerable<string> enumerable = values as string[] ?? values.ToArray();
        var invalid = enumerable.Where(v => !Enum.TryParse<TEnum>(v, true, out _)).ToList();

        if (invalid.Count != 0)
            return Result.Failure<TEnum>($"Unsupported values: {string.Join(", ", invalid)}");

        var combined = enumerable
            .Select(v => Enum.Parse<TEnum>(v, true))
            .Aggregate(default(TEnum), (acc, next) => (TEnum)(object)((int)(object)acc | (int)(object)next));

        return Result.Success(combined);
    }
}