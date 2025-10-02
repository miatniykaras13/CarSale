using CarSale.Application;
using CarSale.Infrastructure.MsSql;

namespace CarSale.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services)
    {
        services.AddApplication();

        services.AddWeb();

        services.AddMsSqlInfrastructure();

        return services;
    }


    private static IServiceCollection AddWeb(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();
        return services;
    }
}