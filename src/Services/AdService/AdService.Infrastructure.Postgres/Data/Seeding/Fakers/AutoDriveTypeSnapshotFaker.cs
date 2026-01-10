using AdService.Domain.ValueObjects;
using Bogus;

namespace AdService.Infrastructure.Postgres.Data.Seeding.Fakers;

public class AutoDriveTypeSnapshotFaker
{
    private static readonly string[] _driveTypes = ["FWD", "RWD", "AWD"];

    public static AutoDriveTypeSnapshot[] Fake(int amount)
    {
        var faker = new Faker<AutoDriveTypeSnapshot>()
            .UseSeed(7)
            .CustomInstantiator(f =>
            {
                var driveType = _driveTypes[f.IndexFaker % _driveTypes.Length];
                var result = AutoDriveTypeSnapshot.Of(f.Random.Int(1), driveType);

                if (result.IsFailure)
                    throw new InvalidOperationException($"AutoDriveTypeSnapshot faker failed: {result.Error.Message}");

                return result.Value;
            });

        return faker.Generate(amount).ToArray();
    }
}