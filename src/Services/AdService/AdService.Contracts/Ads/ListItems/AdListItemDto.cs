using AdService.Contracts.Ads.Default;

namespace AdService.Contracts.Ads.ListItems;

public record AdListItemDto(
    string Title,
    string? Description,
    CarSnapshotListItemDto Car,
    MoneyDto Price);