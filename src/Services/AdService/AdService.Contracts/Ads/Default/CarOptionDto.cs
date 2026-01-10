namespace AdService.Contracts.Ads.Default;

public record CarOptionDto(
    int Id,
    string OptionType,
    string Name,
    string TechnicalName);