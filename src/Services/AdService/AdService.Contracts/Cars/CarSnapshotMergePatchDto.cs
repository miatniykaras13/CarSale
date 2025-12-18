namespace AdService.Contracts.Cars;

public class CarSnapshotMergePatchDto
{
    public int? BrandId { get; set; }

    public int? ModelId { get; set; }

    public int? Year { get; set; }

    public int? GenerationId { get; set; }

    public int? EngineId { get; set; }

    public string? Vin { get; set; }

    public int? Mileage { get; set; }

    public decimal? Consumption { get; set; }

    public string? Color { get; set; }

    public int? HorsePower { get; set; }

    public int? DriveTypeId { get; set; }

    public int? TransmissionTypeId { get; set; }

    public int? BodyTypeId { get; set; }
}