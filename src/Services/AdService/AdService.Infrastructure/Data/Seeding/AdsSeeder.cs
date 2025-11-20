using AdService.Domain.Aggregates;
using CSharpFunctionalExtensions;

namespace AdService.Infrastructure.Data.Seeding;

public class AdsSeeder : ISeeder
{
    public async Task SeedAsync(AppDbContext context)
    {
        if (!context.Set<Ad>().Any())
            return;

        context.Set<Ad>().AddRange();
    }
}