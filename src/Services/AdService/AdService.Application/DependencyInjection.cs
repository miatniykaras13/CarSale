using System.Reflection;
using AdService.Application.FileStorage;
using Microsoft.Extensions.DependencyInjection;

namespace AdService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });


        return services;
    }
}