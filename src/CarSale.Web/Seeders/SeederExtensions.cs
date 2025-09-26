using CarSale.Infrastructure.MsSql;
using CarSale.Infrastructure.MsSql.Seeders;

namespace CarSale.Web.Seeders;

public static class SeederExtensions
{
    public static async Task<WebApplication> UseSeeders(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        IEnumerable<ISeeder> seeders = scope.ServiceProvider.GetServices<ISeeder>();

        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        foreach (ISeeder seeder in seeders)
        {
            await seeder.SeedAsync(context);
        }

        return app;
    }
}