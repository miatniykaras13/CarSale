namespace AdService.Contracts.Ads.Default.Snapshots;

public record SellerSnapshotDto(
    Guid Id,
    string? DisplayName,
    PhoneNumberDto? PhoneNumber);