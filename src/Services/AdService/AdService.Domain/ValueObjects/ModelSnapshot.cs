using CSharpFunctionalExtensions;

namespace AdService.Domain.ValueObjects;

public record ModelSnapshot
{
    public const int MAX_NAME_LENGTH = 200;

    public const int MIN_NAME_LENGTH = 1;

    public int Id { get; private set; }

    public string Name { get; private set; }

    public int BrandId { get; private set; }


    protected ModelSnapshot()
    {
    }

    private ModelSnapshot(
        int id,
        string name,
        int brandId)
    {
        Id = id;
        Name = name;
        BrandId = brandId;
    }

    public static Result<ModelSnapshot, Error> Of(int id, string name, int brandId)
    {
        if (name.Length is > MAX_NAME_LENGTH or < MIN_NAME_LENGTH)
        {
            return Result.Failure<ModelSnapshot, Error>(Error.Domain(
                "model_snapshot.name",
                $"Model name should be between {MIN_NAME_LENGTH} and {MAX_NAME_LENGTH} characters long"));
        }

        ModelSnapshot modelSnapshot = new(id, name, brandId);
        return Result.Success<ModelSnapshot, Error>(modelSnapshot);
    }
}