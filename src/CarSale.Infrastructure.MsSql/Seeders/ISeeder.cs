namespace CarSale.Infrastructure.MsSql.Seeders;

public interface ISeeder
{
    Task SeedAsync(AppDbContext context);
}