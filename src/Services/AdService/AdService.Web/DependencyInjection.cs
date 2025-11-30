using AdService.Application;
using AdService.Infrastructure.FileStorage;
using AdService.Infrastructure.Postgres;

namespace AdService.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddApplication()
            .AddPostgresInfrastructure(configuration)
            .AddFileStorage(configuration)
            .AddWeb();
        return services;
    }

    private static IServiceCollection AddWeb(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
}