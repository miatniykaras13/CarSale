using AdService.Domain.ValueObjects;
using Bogus;

namespace AdService.Infrastructure.Data.Seeding.Fakers;

public class MoneyFaker
{
    public static Money[] Fake(int amount, Currency[] currencies)
    {
        var faker = new Faker<Money>()
            .UseSeed(7)
            .CustomInstantiator(f =>
            {
                var value = f.Random.Int(Money.MIN_AMOUNT + 1, 5_253_255);

                var currency = f.Random.ArrayElement(currencies);

                var result = Money.Of(currency, value);

                if (result.IsFailure)
                    throw new InvalidOperationException($"Money faker failed: {result.Error}");

                return result.Value;
            });

        return faker.Generate(amount).ToArray();
    }
}