using AdService.Contracts.Ads.Default;
using AdService.Contracts.Ads.Default.Snapshots;

namespace AdService.Contracts.Ads.ListItems;

public record AdListItemDto(
    Guid AdId,
    string? Title,
    string? Description,
    string Status,
    CarSnapshotListItemDto? Car,
    SellerSnapshotDto Seller,
    Guid? ImageId,
    List<MoneyDto> Prices,
    LocationDto? Location)
{
    public string? ImageUrl { get; set; }
}