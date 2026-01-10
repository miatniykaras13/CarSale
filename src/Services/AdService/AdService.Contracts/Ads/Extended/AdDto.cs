using AdService.Contracts.Ads.Default;
using AdService.Contracts.Ads.Default.Snapshots;
using AdService.Contracts.Ads.Extended.Snapshots;

namespace AdService.Contracts.Ads.Extended;

public record AdDto(
    Guid AdId,
    string Title,
    string? Description,
    List<MoneyDto> Prices,
    LocationDto Location,
    int Views,
    SellerSnapshotDto Seller,
    CarSnapshotDto Car,
    IEnumerable<string> ImageUrls,
    CommentDto? Comment,
    IEnumerable<CarOptionDto> CarOptions);