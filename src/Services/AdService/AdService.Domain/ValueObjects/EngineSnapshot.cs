using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace AdService.Domain.ValueObjects;

public record EngineSnapshot
{
    public const int MAX_NAME_LENGTH = 200;
    public const int MIN_NAME_LENGTH = 1;
    public const int MIN_HORSE_POWER = 10;
    public const int MAX_HORSE_POWER = 1500;

    public int Id { get; private set; }

    public int HorsePower { get; private set; }

    public FuelTypeSnapshot FuelType { get; private set; }

    public string Name { get; private set; }

    public int GenerationId { get; private set; }


    protected EngineSnapshot()
    {
    }

    [JsonConstructor]
    private EngineSnapshot(
        int id,
        string name,
        int horsePower,
        FuelTypeSnapshot fuelType,
        int generationId)
    {
        Id = id;
        Name = name;
        HorsePower = horsePower;
        FuelType = fuelType;
        GenerationId = generationId;
    }

    public static Result<EngineSnapshot, Error> Of(int id, string name, int horsePower, FuelTypeSnapshot fuelType, int generationId)
    {
        if (name.Length is > MAX_NAME_LENGTH or < MIN_NAME_LENGTH)
        {
            return Result.Failure<EngineSnapshot, Error>(Error.Domain(
                "engine_snapshot.name",
                $"Engine name should be between {MIN_NAME_LENGTH} and {MAX_NAME_LENGTH} characters long"));
        }

        if (horsePower is < MIN_HORSE_POWER or > MAX_HORSE_POWER)
        {
            return Result.Failure<EngineSnapshot, Error>(Error.Domain(
                "engine_snapshot.horse_power",
                $"Engine horse power should be between {MIN_HORSE_POWER} and {MAX_HORSE_POWER} characters long"));
        }

        EngineSnapshot engineSnapshot = new(id, name, horsePower, fuelType, generationId);
        return Result.Success<EngineSnapshot, Error>(engineSnapshot);
    }
}