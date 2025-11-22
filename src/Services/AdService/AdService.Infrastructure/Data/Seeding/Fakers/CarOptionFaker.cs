using AdService.Domain.Entities;
using AdService.Domain.Enums;
using Bogus;

namespace AdService.Infrastructure.Data.Seeding.Fakers;

public class CarOptionFaker
{
    public static CarOption[] Fake(int amount)
    {
        var faker = new Faker<CarOption>()
            .UseSeed(7)
            .CustomInstantiator(f =>
            {
                var name = f.Lorem.Word();
                if (name.Length < CarOption.MIN_NAME_LENGTH)
                {
                    name = name + f.Random.String2(CarOption.MIN_NAME_LENGTH - name.Length);
                }
                if (name.Length > CarOption.MAX_NAME_LENGTH)
                {
                    name = name[..CarOption.MAX_NAME_LENGTH];
                }

                var technicalName = name.ToLowerInvariant();
                if (technicalName.Length < CarOption.MIN_TECHNAME_LENGTH)
                {
                    technicalName = technicalName + f.Random.String2(CarOption.MIN_TECHNAME_LENGTH - technicalName.Length);
                }
                if (technicalName.Length > CarOption.MAX_TECHNAME_LENGTH)
                {
                    technicalName = technicalName[..CarOption.MAX_TECHNAME_LENGTH];
                }

                var optionType = f.PickRandom<OptionType>();

                var result = CarOption.Create(optionType, name, technicalName);

                if (result.IsFailure)
                    throw new InvalidOperationException($"CarOption faker failed: {result.Error}");

                return result.Value;
            });

        return faker.Generate(amount).ToArray();
    }
}