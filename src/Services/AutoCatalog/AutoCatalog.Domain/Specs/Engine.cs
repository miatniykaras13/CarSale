using System.Text.Json.Serialization;
using AutoCatalog.Domain.Enums;
using AutoCatalog.Domain.Transport.Cars;

namespace AutoCatalog.Domain.Specs;

public class Engine
{
    public int Id { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required FuelType FuelType { get; init; }


    public required int GenerationId { get; init; }

    public Generation Generation { get; init; } = null!;


    public required string Name { get; init; }

    public required float Volume { get; init; }

    public required int HorsePower { get; init; }

    public required int TorqueNm { get; init; }

    public List<Car> Cars { get; init; } = [];
}