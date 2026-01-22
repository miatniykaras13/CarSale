using CSharpFunctionalExtensions;

namespace AdService.Domain.ValueObjects;

public record AutoDriveTypeSnapshot
{
    public const int MAX_NAME_LENGTH = 200;

    public const int MIN_NAME_LENGTH = 1;

    public int Id { get; private set; }

    public string Name { get; private set; }


    protected AutoDriveTypeSnapshot()
    {
    }

    private AutoDriveTypeSnapshot(
        int id,
        string name)
    {
        Id = id;
        Name = name;
    }

    public static Result<AutoDriveTypeSnapshot, Error> Of(int id, string name)
    {
        if (name.Length is > MAX_NAME_LENGTH or < MIN_NAME_LENGTH)
        {
            return Result.Failure<AutoDriveTypeSnapshot, Error>(Error.Domain(
                "drive_type_snapshot.name",
                $"Auto drive type name should be between {MIN_NAME_LENGTH} and {MAX_NAME_LENGTH} characters long"));
        }

        AutoDriveTypeSnapshot driveSnapshot = new(id, name);
        return Result.Success<AutoDriveTypeSnapshot, Error>(driveSnapshot);
    }
}