using System.Text.Json.Serialization;
using AutoCatalog.Application;
using AutoCatalog.Infrastructure;
using BuildingBlocks.Exceptions.Handlers;
using Microsoft.AspNetCore.Http.Json;

namespace AutoCatalog.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddWeb();
        services.AddApplication();
        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString(nameof(AppDbContext))!);
        return services;
    }

    private static IServiceCollection AddWeb(this IServiceCollection services)
    {
        services.AddCarter();
        services.AddOpenApi();
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        services.AddExceptionHandler<CustomExceptionHandler>();

        return services;
    }
}