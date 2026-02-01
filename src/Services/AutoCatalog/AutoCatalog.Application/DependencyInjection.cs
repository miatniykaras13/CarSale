using AutoCatalog.Application.Options;
using BuildingBlocks.Behaviors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutoCatalog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            c.AddOpenBehavior(typeof(LoggingBehavior<,>));
            c.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.Configure<FileStorageOptions>(configuration.GetSection("FileStorage"));
        return services;
    }
}