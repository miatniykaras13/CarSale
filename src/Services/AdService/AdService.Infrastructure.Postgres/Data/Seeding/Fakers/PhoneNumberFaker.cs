using AdService.Domain.ValueObjects;
using Bogus;

namespace AdService.Infrastructure.Postgres.Data.Seeding.Fakers;

public class PhoneNumberFaker
{
    private static readonly string[] _phoneNumbers = ["+375297304300", "+375257150074", "+375292925857"];

    public static PhoneNumber[] Fake(int amount)
    {
        var faker = new Faker<PhoneNumber>()
            .UseSeed(7)
            .CustomInstantiator(f =>
            {
                string number = f.Random.ArrayElement(_phoneNumbers);

                var result = PhoneNumber.Of(number);

                if (result.IsFailure)
                    throw new InvalidOperationException($"PhoneNumber faker failed: {result.Error.Message}");

                return result.Value;
            });

        return faker.Generate(amount).ToArray();
    }
}