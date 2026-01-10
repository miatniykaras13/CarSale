using AdService.Domain.ValueObjects;
using Bogus;

namespace AdService.Infrastructure.Postgres.Data.Seeding.Fakers;

public class LocationFaker
{
    public static Location[] Fake(int amount)
    {
        var faker = new Faker<Location>()
            .UseSeed(7)
            .CustomInstantiator(f =>
            {
                var city = f.Address.City();
                var region = f.Address.State();

                var result = Location.Of(region, city);

                if (result.IsFailure)
                    throw new InvalidOperationException($"Location faker failed: {result.Error.Message}");

                return result.Value;
            });

        return faker.Generate(amount).ToArray();
    }
}