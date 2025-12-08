using AdService.Domain.ValueObjects;
using Bogus;

namespace AdService.Infrastructure.Postgres.Data.Seeding.Fakers;

public class SellerSnapshotFaker
{
    public static SellerSnapshot[] Fake(int amount)
    {
        var faker = new Faker<SellerSnapshot>()
            .UseSeed(7)
            .CustomInstantiator(f =>
            {
                var displayName = f.Internet.UserName();

                if (displayName.Length > SellerSnapshot.MAX_NAME_LENGTH)
                {
                    displayName = displayName[..SellerSnapshot.MAX_NAME_LENGTH];
                }

                var imageId = f.Random.Guid();
                var registrationDate = DateTime.UtcNow;

                var result = SellerSnapshot.Of(f.Random.Guid(), displayName, registrationDate, imageId);

                if (result.IsFailure)
                    throw new InvalidOperationException($"SellerSnapshot faker failed: {result.Error}");

                return result.Value;
            });

        return faker.Generate(amount).ToArray();
    }
}