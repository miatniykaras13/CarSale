using AdService.Domain.ValueObjects;
using Bogus;

namespace AdService.Infrastructure.Postgres.Data.Seeding.Fakers;

public class CurrencyFaker
{
    public static Currency[] Fake(int amount)
    {
        var faker = new Faker<Currency>()
            .UseSeed(7)
            .CustomInstantiator(f =>
            {
                var code = f.PickRandom(Currency.GetSupportedCurrencies);
                var result = Currency.Of(code);

                if (result.IsFailure)
                    throw new InvalidOperationException($"Currency faker failed: {result.Error}");

                return result.Value;
            });

        return faker.Generate(amount).ToArray();
    }
}