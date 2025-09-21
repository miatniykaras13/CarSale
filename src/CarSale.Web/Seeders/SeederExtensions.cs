using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarSale.Infrastructure;
using CarSale.Infrastructure.MsSql;
using CarSale.Infrastructure.MsSql.Seeders;

namespace CarSale.Web.Seeders
{
    public static class SeederExtensions
    {
        public async static Task<WebApplication> UseSeeders(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var seeders = scope.ServiceProvider.GetServices<ISeeder>();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(context);
            }

            return app;
        }
    }
}
