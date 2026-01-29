using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace AdService.Domain.ValueObjects;

public record BrandSnapshot
{
    public const int MAX_NAME_LENGTH = 200;

    public const int MIN_NAME_LENGTH = 1;

    public int Id { get; private set; }

    public string Name { get; private set; }


    protected BrandSnapshot()
    {
    }

    [JsonConstructor]
    private BrandSnapshot(
        int id,
        string name)
    {
        Id = id;
        Name = name;
    }

    public static Result<BrandSnapshot, Error> Of(int id, string name)
    {
        if (name.Length is > MAX_NAME_LENGTH or < MIN_NAME_LENGTH)
        {
            return Result.Failure<BrandSnapshot, Error>(Error.Domain(
                "brand_snapshot.name",
                $"Brand name should be between {MIN_NAME_LENGTH} and {MAX_NAME_LENGTH} characters long"));
        }

        BrandSnapshot brandSnapshot = new(id, name);
        return Result.Success<BrandSnapshot, Error>(brandSnapshot);
    }
}