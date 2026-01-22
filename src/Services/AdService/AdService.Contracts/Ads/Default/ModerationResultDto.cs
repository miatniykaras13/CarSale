using AdService.Contracts.Ads.Default;
using AdService.Domain.Enums;

namespace AdService.Contracts.Ads.Default;

public record ModerationResultDto(DenyReason? DenyReason, string? Message);