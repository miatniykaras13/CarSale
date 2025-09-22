﻿using CarSale.Application.Ads;
using CarSale.Infrastructure.MsSql;
using Microsoft.Extensions.DependencyInjection;

namespace CarSale.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services)
    {
        services.AddApplication();
        
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