using AdService.Domain.Enums;

namespace AdService.Contracts.Ads.Default;

public record CarOptionDto(
    int Id,
    OptionType OptionType,
    string Name,
    string TechnicalName);