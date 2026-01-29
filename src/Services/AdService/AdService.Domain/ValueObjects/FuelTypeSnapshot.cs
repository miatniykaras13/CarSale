using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace AdService.Domain.ValueObjects;

public record FuelTypeSnapshot
{
    public const int MAX_NAME_LENGTH = 200;

    public const int MIN_NAME_LENGTH = 3;

    public int Id { get; private set; }

    public string Name { get; private set; }


    protected FuelTypeSnapshot()
    {
    }

    [JsonConstructor]
    private FuelTypeSnapshot(
        int id,
        string name)
    {
        Id = id;
        Name = name;
    }

    public static Result<FuelTypeSnapshot, Error> Of(int id, string name)
    {
        if (name.Length is > MAX_NAME_LENGTH or < MIN_NAME_LENGTH)
        {
            return Result.Failure<FuelTypeSnapshot, Error>(Error.Domain(
                "fuel_type_snapshot.name",
                $"Fuel type name should be between {MIN_NAME_LENGTH} and {MAX_NAME_LENGTH} characters long"));
        }

        FuelTypeSnapshot fuelSnapshot = new(id, name);
        return Result.Success<FuelTypeSnapshot, Error>(fuelSnapshot);
    }
}