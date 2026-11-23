using Carter;
using Microsoft.Extensions.DependencyInjection;

namespace ProfileService.Presenters;

public static class DependencyInjection
{
    public static IServiceCollection AddPresenters(this IServiceCollection services)
    {
        services.AddCarter();
        return services;
    }
}
