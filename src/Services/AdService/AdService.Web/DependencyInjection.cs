using AdService.Application;
using AdService.Infrastructure.AutoCatalog;
using AdService.Infrastructure.Core;
using AdService.Infrastructure.FileStorage;
using AdService.Infrastructure.Postgres;
using AdService.Infrastructure.ProfileService;
using AdService.Infrastructure.Redis;
using AdService.Presenters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AdService.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddPostgresInfrastructure(configuration)
            .AddApplication(configuration)
            .AddBackgroundServices()
            .AddFileStorage(configuration)
            .AddProfileServiceCommunication()
            .AddAutoCatalogCommunication()
            .AddHybridCachingWithFusionAndRedis(configuration)
            .AddPresenters()
            .AddWeb()
            .ConfigureOptions();
        return services;
    }


    public static IServiceCollection AddApiAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSwaggerGenWithAuth(configuration);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.Audience = configuration["Authentication:Audience"];
                o.MetadataAddress = configuration["Authentication:MetadataAddress"]!;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["Authentication:ValidIssuer"],
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy =>
            {
                policy.RequireClaim("roles", "autocatalog_admin");
            });
        });
        return services;
    }

    private static IServiceCollection AddWeb(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        return services;
    }

    private static IServiceCollection ConfigureOptions(this IServiceCollection services)
    {
        services.Configure<FormOptions>(o =>
        {
            o.MultipartBodyLengthLimit = long.MaxValue;
            o.MemoryBufferThreshold = int.MaxValue;
        });

        services.Configure<JsonOptions>(o =>
        {
            o.SerializerOptions.IncludeFields = true;
        });
        return services;
    }

    private static IServiceCollection AddSwaggerGenWithAuth(
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
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(configuration["Keycloak:AuthorizationUrl"]!),
                            TokenUrl = new Uri(configuration["Keycloak:TokenUrl"]!),
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
                    },
                    ["openid", "profile"]
                },
            };

            o.AddSecurityRequirement(securityRequirement);
        });
        return services;
    }
}