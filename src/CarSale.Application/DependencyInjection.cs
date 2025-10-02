using CarSale.Application.Ads;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CarSale.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddScoped<IAdsService, AdsService>();

        return services;
    }
}