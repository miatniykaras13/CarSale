using System.Text.Json.Serialization;
using AutoCatalog.Application;
using AutoCatalog.Infrastructure;
using BuildingBlocks.Exceptions.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;

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

    public static IServiceCollection AddSwaggerGenWithAuth(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSwaggerGen(o =>
        {
            o.CustomSchemaIds(x => x.FullName!.Replace("+", "-"));

            o.AddSecurityDefinition(
                "Keycloak",
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(configuration["Keycloak:AuthorizationUrl"]!),
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "openid" }, { "profile", "profile" },
                            },
                        },
                    },
                });

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Id = "Keycloak", Type = ReferenceType.SecurityScheme },
                        In = ParameterLocation.Header,
                        Name = "Bearer",
                        Scheme = "Bearer",
                    },
                    []
                },
            };

            o.AddSecurityRequirement(securityRequirement);
        });
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
        services.AddEndpointsApiExplorer();
        return services;
    }
}