using AdService.Contracts.Ads.Default.Snapshots;

namespace AdService.Contracts.Ads.Extended.Snapshots;

public record CarSnapshotDto(
    Guid CarId,
    BrandSnapshotDto Brand,
    ModelSnapshotDto Model,
    GenerationSnapshotDto Generation,
    EngineSnapshotDto Engine,
    AutoDriveTypeSnapshotDto DriveType,
    TransmissionTypeSnapshotDto TransmissionType,
    BodyTypeSnapshotDto BodyType,
    int Year,
    decimal? Consumption,
    string? Vin,
    int? Mileage,
    string? Color);