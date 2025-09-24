using Microsoft.AspNetCore.OpenApi;

namespace AutoCatalog.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services)
    {
        services.AddCarter();
        services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });
        services.AddOpenApi();
        return services;
    }
}