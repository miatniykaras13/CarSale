using CarSale.Domain.Ads.Enums;
using CSharpFunctionalExtensions;

namespace CarSale.Domain.Ads.ValueObjects;

public record CarConfiguration(
    InteriorType Interior,
    SafetyOptions Safety,
    ComfortOptions Comfort,
    AssistanceOptions Assistance,
    OpticsType Optics
)
{
    public static Result<CarConfiguration> Of(
        InteriorType interiorType,
        SafetyOptions safetyOptions,
        ComfortOptions comfortOptions,
        AssistanceOptions assistanceSystems,
        OpticsType opticsType) =>
        Result.Success(
            new CarConfiguration(
                interiorType,
                safetyOptions,
                comfortOptions,
                assistanceSystems,
                opticsType));
}