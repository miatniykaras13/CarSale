using System.Text.Json.Serialization;
using AutoCatalog.Application;
using AutoCatalog.Infrastructure;
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
        return services;
    }

    private static IServiceCollection AddWeb(this IServiceCollection services)
    {
        services.AddCarter();
        services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
        });
        services.AddOpenApi();
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        return services;
    }
}