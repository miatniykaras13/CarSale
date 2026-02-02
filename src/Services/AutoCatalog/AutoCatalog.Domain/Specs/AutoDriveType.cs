using AutoCatalog.Domain.Transport.Cars;

namespace AutoCatalog.Domain.Specs;

public class AutoDriveType
{
    public int Id { get; init; }

    public required string Name { get; set; }

    public List<Car> Cars { get; init; } = [];
}