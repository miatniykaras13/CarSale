namespace AdService.Contracts.Ads.ListItems;

public record CarSnapshotListItemDto(
    Guid CarId,
    string Brand,
    string Model,
    string Generation,
    int Year,
    string DriveType,
    string TransmissionType,
    string FuelType,
    string BodyType);