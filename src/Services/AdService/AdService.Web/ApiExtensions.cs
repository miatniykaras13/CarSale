using AdService.Infrastructure.Data;

namespace AdService.Web;

public static class ApiExtensions
{
    public static async Task UseAsyncSeeding(this WebApplication app)
    {
        await using var serviceScope = app.Services.CreateAsyncScope();
        await using var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.EnsureCreatedAsync();
    }
}