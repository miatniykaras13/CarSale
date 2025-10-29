using AdService.Domain.Ads.Enums;
using CSharpFunctionalExtensions;

namespace AdService.Domain.Ads.ValueObjects;

public record CarConfiguration
{
    public InteriorType Interior { get; private set; }

    public SafetyOptions Safety { get; private set; }

    public ComfortOptions Comfort { get; private set; }

    public AssistanceOptions Assistance { get; private set; }

    public OpticsType Optics { get; private set; }

    protected CarConfiguration()
    {
    }

    private CarConfiguration(
        InteriorType interior,
        SafetyOptions safety,
        ComfortOptions comfort,
        AssistanceOptions assistance,
        OpticsType optics)
    {
        Interior = interior;
        Safety = safety;
        Comfort = comfort;
        Assistance = assistance;
        Optics = optics;
    }

    public static Result<CarConfiguration, Error> Of(
        InteriorType interiorType,
        SafetyOptions safetyOptions,
        ComfortOptions comfortOptions,
        AssistanceOptions assistanceSystems,
        OpticsType opticsType) =>
        Result.Success<CarConfiguration, Error>(
            new CarConfiguration(
                interiorType,
                safetyOptions,
                comfortOptions,
                assistanceSystems,
                opticsType));
}