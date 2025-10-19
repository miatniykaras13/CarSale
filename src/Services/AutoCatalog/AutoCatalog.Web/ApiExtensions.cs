using AutoCatalog.Infrastructure;
using Scalar.AspNetCore;

namespace AutoCatalog.Web;

public static class ApiExtensions
{
    public static WebApplication MapScalar(this WebApplication app)
    {
        app.MapScalarApiReference("/docs", options =>
        {
            options
                .WithTitle("Auto catalog API")
                .WithTheme(ScalarTheme.DeepSpace)
                .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Fetch)
                .WithOpenApiRoutePattern("/openapi/{documentName}.json");
        });
        return app;
    }

    public static async Task UseAsyncSeeding(this WebApplication app)
    {
        await using var serviceScope = app.Services.CreateAsyncScope();
        await using var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.EnsureCreatedAsync();
    }
}