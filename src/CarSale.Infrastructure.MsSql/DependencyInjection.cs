using CarSale.Application.Ads.Interfaces;
using CarSale.Infrastructure.MsSql.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CarSale.Infrastructure.MsSql;

public static class DependencyInjection
{
    public static IServiceCollection AddMsSqlInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IAdsRepository, AdsEfRepository>();

        services.AddDbContext<AppDbContext>();

        return services;
    }
}