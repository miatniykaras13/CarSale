using AdService.Domain.ValueObjects;
using Bogus;

namespace AdService.Infrastructure.Postgres.Data.Seeding.Fakers;

public class TransmissionTypeSnapshotFaker
{
    private static readonly string[] _transmissionTypes = ["Automatic", "Manual", "CVT"];

    public static TransmissionTypeSnapshot[] Fake(int amount)
    {
        var faker = new Faker<TransmissionTypeSnapshot>()
            .UseSeed(7)
            .CustomInstantiator(f =>
            {
                var transmissionType = _transmissionTypes[f.IndexFaker % _transmissionTypes.Length];
                var result = TransmissionTypeSnapshot.Of(f.Random.Int(1), transmissionType);

                if (result.IsFailure)
                {
                    throw new InvalidOperationException(
                        $"TransmissionTypeSnapshot faker failed: {result.Error.Message}");
                }

                return result.Value;
            });

        return faker.Generate(amount).ToArray();
    }
}