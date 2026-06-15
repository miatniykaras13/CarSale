using AdService.Infrastructure.Postgres.Data;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace AdService.Web;

public static class ApiExtensions
{
    public static async Task UseAsyncSeeding(this WebApplication app)
    {
        await using var serviceScope = app.Services.CreateAsyncScope();
        await using var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.EnsureCreatedAsync();
    }

    public static void UseServiceHealthChecks(this WebApplication app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });
    }
}