using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using ProfileService.Infrastructure.Postgres.Data;

namespace ProfileService.Web;

public static class ApiExtensions
{
    public static void UseServiceHealthChecks(this WebApplication app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });
    }

    public static async Task UseAsyncMigrations(this WebApplication app)
    {
        await using var serviceScope = app.Services.CreateAsyncScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.MigrateAsync();
    }
}
