namespace AdService.Contracts.Ads.ListItems;

public record AdsPageDto(
    IReadOnlyList<AdListItemDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount);
