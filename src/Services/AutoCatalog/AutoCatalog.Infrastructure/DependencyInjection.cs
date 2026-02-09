using System.Net.Security;
using AutoCatalog.Application.Abstractions.FileStorage;
using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Infrastructure.FileStorage;
using AutoCatalog.Infrastructure.Repositories.Specs;
using AutoCatalog.Infrastructure.Repositories.Transport;
using AutoCatalog.Infrastructure.Seeding;
using FileManagement.Grpc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutoCatalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt
                .UseNpgsql(configuration.GetConnectionString(nameof(AppDbContext)))
                .SeedDatabase();
        });

        services.AddFileStorage(configuration);

        services.AddScoped<ICarsRepository, CarsRepository>();
        services.AddScoped<IBrandsRepository, BrandsRepository>();
        services.AddScoped<IModelsRepository, ModelsRepository>();
        services.AddScoped<IGenerationsRepository, GenerationsRepository>();
        services.AddScoped<IEnginesRepository, EnginesRepository>();
        services.AddScoped<IBodyTypesRepository, BodyTypesRepository>();
        services.AddScoped<ITransmissionTypesRepository, TransmissionTypesRepository>();
        services.AddScoped<IAutoDriveTypesRepository, AutoDriveTypesRepository>();
        services.AddScoped<IFuelTypesRepository, FuelTypesRepository>();
        return services;
    }

    private static IServiceCollection AddFileStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<FileManager.FileManagerClient>(o =>
            {
                o.Address = new Uri(configuration["FileStorage:Endpoint"]!);
            })
            .ConfigureChannel(o =>
            {
                o.MaxReceiveMessageSize = null;
                o.MaxSendMessageSize = null;
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var sockets = new SocketsHttpHandler
                {
                    EnableMultipleHttp2Connections = true,
                    SslOptions = new SslClientAuthenticationOptions
                    {
                        // делаю это, потому что не смог нормально настроить сертификаты
                        #pragma warning disable CA5359
                        RemoteCertificateValidationCallback = (sender, cert, chain, errors) => true,
                        #pragma warning restore CA5359
                    },
                };
                return sockets;
            });
        services.AddScoped<IFileStorage, MinioFileStorage>();
        return services;
    }
}