namespace CarSale.Contracts.Ads;

public record CarConfigurationDto(
    List<string> AssistanceOptions,
    List<string> ComfortOptions,
    List<string> SafetyOptions,
    string OpticsType,
    string InteriorType
);