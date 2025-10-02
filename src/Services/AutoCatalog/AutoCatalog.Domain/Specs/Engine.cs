using System.Text.Json.Serialization;
using AutoCatalog.Domain.Enums;
using AutoCatalog.Domain.Transport.Cars;

namespace AutoCatalog.Domain.Specs;

public class Engine
{
    public int Id { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required FuelType FuelType { get; set; }


    public required int GenerationId { get; set; }

    public Generation Generation { get; set; } = null!;


    public required string Name { get; set; }

    public required float Volume { get; set; }

    public required int HorsePower { get; set; }

    public required int TorqueNm { get; set; }

    public List<Car> Cars { get; set; } = [];
}