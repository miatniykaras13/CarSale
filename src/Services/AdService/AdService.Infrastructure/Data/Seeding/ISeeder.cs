namespace AdService.Infrastructure.Data.Seeding;

public interface ISeeder
{
    Task SeedAsync(AppDbContext context);
}