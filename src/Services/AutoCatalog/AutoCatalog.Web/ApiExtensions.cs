using Scalar.AspNetCore;

namespace AutoCatalog.Web;

public static class ApiExtensions
{
    public static WebApplication MapScalar(this WebApplication app)
    {
        app.MapScalarApiReference("/docs", options =>
        {
            options
                .WithTitle("CarSale API")
                .WithTheme(ScalarTheme.Solarized)
                .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Fetch)
                .WithOpenApiRoutePattern("/openapi/{documentName}.json");
        });
        return app;
    }
}