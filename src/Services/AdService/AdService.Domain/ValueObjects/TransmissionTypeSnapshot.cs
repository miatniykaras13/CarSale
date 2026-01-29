using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace AdService.Domain.ValueObjects;

public record TransmissionTypeSnapshot
{
    public const int MAX_NAME_LENGTH = 200;

    public const int MIN_NAME_LENGTH = 1;

    public int Id { get; private set; }

    public string Name { get; private set; }


    protected TransmissionTypeSnapshot()
    {
    }

    [JsonConstructor]
    private TransmissionTypeSnapshot(
        int id,
        string name)
    {
        Id = id;
        Name = name;
    }

    public static Result<TransmissionTypeSnapshot, Error> Of(int id, string name)
    {
        if (name.Length is > MAX_NAME_LENGTH or < MIN_NAME_LENGTH)
        {
            return Result.Failure<TransmissionTypeSnapshot, Error>(Error.Domain(
                "transmission_type_snapshot.name",
                $"Transmission type name should be between {MIN_NAME_LENGTH} and {MAX_NAME_LENGTH} characters long"));
        }

        TransmissionTypeSnapshot transmissionSnapshot = new(id, name);
        return Result.Success<TransmissionTypeSnapshot, Error>(transmissionSnapshot);
    }
}