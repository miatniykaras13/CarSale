namespace ProfileService.Application.Dtos;

public record CarSnapshotDto(
    string Brand,
    string Model,
    string Generation,
    int Year,
    string DriveType,
    string TransmissionType,
    double EngineVolume,
    string FuelType,
    string BodyType);
