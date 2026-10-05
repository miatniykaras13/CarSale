using Microsoft.EntityFrameworkCore;
using ProfileService.Infrastructure.Postgres.Data;

namespace ProfileService.Web;

public static class ApiExtensions
{
    public static async Task UseAsyncMigrations(this WebApplication app)
    {
        await using var serviceScope = app.Services.CreateAsyncScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.MigrateAsync();
    }
}
