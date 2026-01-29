using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace AdService.Domain.ValueObjects;

public record GenerationSnapshot
{
    public const int MAX_NAME_LENGTH = 200;

    public const int MIN_NAME_LENGTH = 1;

    public int Id { get; private set; }

    public string Name { get; private set; }

    public int ModelId { get; private set; }

    public int YearFrom { get; private set; }

    public int YearTo { get; private set; }


    protected GenerationSnapshot()
    {
    }

    [JsonConstructor]
    private GenerationSnapshot(
        int id,
        string name,
        int modelId,
        int yearFrom,
        int yearTo)
    {
        Id = id;
        Name = name;
        ModelId = modelId;
        YearFrom = yearFrom;
        YearTo = yearTo;
    }

    public static Result<GenerationSnapshot, Error> Of(int id, string name, int modelId, int yearFrom, int yearTo)
    {
        if (name.Length is > MAX_NAME_LENGTH or < MIN_NAME_LENGTH)
        {
            return Result.Failure<GenerationSnapshot, Error>(Error.Domain(
                "generation_snapshot.name",
                $"Generation name should be between {MIN_NAME_LENGTH} and {MAX_NAME_LENGTH} characters long"));
        }

        GenerationSnapshot generationSnapshot = new(id, name, modelId, yearFrom, yearTo);
        return Result.Success<GenerationSnapshot, Error>(generationSnapshot);
    }
}