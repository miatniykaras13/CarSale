using CSharpFunctionalExtensions;

namespace AdService.Application.Shared.Factories;

public class CarConfigurationFactory
{
    public static Result<CarConfiguration> FromDto(CarConfigurationDto configDto)
    {
        InteriorType interiorType = Enum.Parse<InteriorType>(configDto.InteriorType);
        OpticsType opticsType = Enum.Parse<OpticsType>(configDto.OpticsType);
        Result<SafetyOptions> safetyOptionsResult = ParseFlags<SafetyOptions>(configDto.SafetyOptions);
        Result<ComfortOptions> comfortOptionsResult = ParseFlags<ComfortOptions>(configDto.ComfortOptions);
        Result<AssistanceOptions> assistanceOptionsResult = ParseFlags<AssistanceOptions>(configDto.AssistanceOptions);

        Result combinedResult = Result.Combine(safetyOptionsResult, comfortOptionsResult, assistanceOptionsResult);

        if (combinedResult.IsFailure)
        {
            return Result.Failure<CarConfiguration>(combinedResult.Error);
        }

        Result<CarConfiguration> configResult = CarConfiguration.Of(
            interiorType,
            safetyOptionsResult.Value,
            comfortOptionsResult.Value,
            assistanceOptionsResult.Value,
            opticsType);

        if (configResult.IsFailure)
        {
            return Result.Failure<CarConfiguration>(configResult.Error);
        }

        return Result.Success(configResult.Value);
    }

    private static Result<TEnum> ParseFlags<TEnum>(IEnumerable<string> values)
        where TEnum : struct, Enum
    {
        IEnumerable<string> enumerable = values as string[] ?? values.ToArray();
        List<string> invalid = enumerable.Where(v => !Enum.TryParse<TEnum>(v, true, out _)).ToList();

        if (invalid.Count != 0)
        {
            return Result.Failure<TEnum>($"Unsupported values: {string.Join(", ", invalid)}");
        }

        TEnum combined = enumerable
            .Select(v => Enum.Parse<TEnum>(v, true))
            .Aggregate(default(TEnum), (acc, next) => (TEnum)(object)((int)(object)acc | (int)(object)next));

        return Result.Success(combined);
    }
}