using AutoCatalog.Application.Abstractions;
using AutoCatalog.Infrastructure.Repositories;
using AutoCatalog.Infrastructure.Repositories.Specs;
using AutoCatalog.Infrastructure.Repositories.Transport;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutoCatalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString(nameof(AppDbContext)));
        });

        services.AddScoped<ICarsRepository, CarsRepository>();
        services.AddScoped<IBrandsRepository, BrandsRepository>();
        services.AddScoped<IModelsRepository, ModelsRepository>();
        services.AddScoped<IGenerationsRepository, GenerationsRepository>();
        services.AddScoped<IEnginesRepository, EnginesRepository>();
        return services;
    }
}