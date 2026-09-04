using ProfileService.Infrastructure.Postgres;

namespace ProfileService.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddPostgresInfrastructure(configuration);
        return services;
    }
}