using CSharpFunctionalExtensions;

namespace AdService.Domain.ValueObjects;

public record BodyTypeSnapshot
{
    public const int MAX_NAME_LENGTH = 200;

    public const int MIN_NAME_LENGTH = 1;

    public int Id { get; private set; }

    public string Name { get; private set; }


    protected BodyTypeSnapshot()
    {
    }

    private BodyTypeSnapshot(
        int id,
        string name)
    {
        Id = id;
        Name = name;
    }

    public static Result<BodyTypeSnapshot, Error> Of(int id, string name)
    {
        if (name.Length is > MAX_NAME_LENGTH or < MIN_NAME_LENGTH)
        {
            return Result.Failure<BodyTypeSnapshot, Error>(Error.Domain(
                "body_type_snapshot.name",
                $"Body type name should be between {MIN_NAME_LENGTH} and {MAX_NAME_LENGTH} characters long"));
        }

        BodyTypeSnapshot bodySnapshot = new(id, name);
        return Result.Success<BodyTypeSnapshot, Error>(bodySnapshot);
    }
}