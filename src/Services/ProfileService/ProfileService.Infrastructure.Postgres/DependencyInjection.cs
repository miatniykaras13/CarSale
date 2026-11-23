using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfileService.Application.Abstractions.Data;
using ProfileService.Infrastructure.Postgres.Data;
using ProfileService.Infrastructure.Postgres.Extensions;

namespace ProfileService.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddPostgresInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options
                .UseNpgsql(configuration.GetConnectionString(nameof(AppDbContext)))
                .AddInterceptors(sp.GetServices<ISaveChangesInterceptor>())
                .SeedDatabase();
        });

        services.AddScoped<IAppDbContext, AppDbContext>();

        return services;
    }

    public static IHealthChecksBuilder AddPostgresHealthCheck(
        this IHealthChecksBuilder builder,
        IConfiguration configuration)
    {
        builder
            .AddNpgSql(
                connectionString: configuration.GetConnectionString(nameof(AppDbContext)) ??
                                  throw new InvalidOperationException($"Connection string for '{nameof(AppDbContext)}' is not configured."),
                name: "postgres",
                tags: ["ready", "db"]);
        return builder;
    }
}
