using AutoCatalog.Domain.Transport.Cars;

namespace AutoCatalog.Domain.Specs;

public class TransmissionType
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public List<Car> Cars { get; init; } = [];
}