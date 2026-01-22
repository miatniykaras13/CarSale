namespace AdService.Contracts.Ads.Default.Snapshots;

public record EngineSnapshotDto(int Id, string Name, FuelTypeSnapshotDto FuelType, int HorsePower);