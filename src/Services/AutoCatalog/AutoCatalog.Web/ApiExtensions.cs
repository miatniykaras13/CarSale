using AutoCatalog.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
                .WithTheme(ScalarTheme.DeepSpace)
                .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Fetch)
                .WithOpenApiRoutePattern("/openapi/{documentName}.json");
        });
        return app;
    }

    public static WebApplication UseExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(exceptionHandlerApp =>
            exceptionHandlerApp.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                if (exception == null)
                    return;
                var problemDetails = new ProblemDetails
                {
                    Title = exception.Message,
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = exception.StackTrace,
                };
                var logger = context.RequestServices.GetService<ILogger<Program>>()!;
                logger.LogError(exception, exception.Message);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/problem+json";

                await context.Response.WriteAsJsonAsync(problemDetails);
            }));
        return app;
    }
}