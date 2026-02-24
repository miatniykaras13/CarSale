using System.Net.Security;
using System.Reflection;
using AutoCatalog.Application.Abstractions.FileStorage;
using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Infrastructure.FileStorage;
using AutoCatalog.Infrastructure.Repositories.Specs;
using AutoCatalog.Infrastructure.Repositories.Transport;
using AutoCatalog.Infrastructure.Seeding;
using FileManagement.Grpc;
using MassTransit;
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

        services
            .AddFileStorage(configuration)
            .AddMessageBus(configuration, typeof(Application.DependencyInjection).Assembly);

        services
            .AddScoped<ICarsRepository, CarsRepository>()
            .AddScoped<IBrandsRepository, BrandsRepository>()
            .AddScoped<IModelsRepository, ModelsRepository>()
            .AddScoped<IGenerationsRepository, GenerationsRepository>()
            .AddScoped<IEnginesRepository, EnginesRepository>()
            .AddScoped<IBodyTypesRepository, BodyTypesRepository>()
            .AddScoped<ITransmissionTypesRepository, TransmissionTypesRepository>()
            .AddScoped<IAutoDriveTypesRepository, AutoDriveTypesRepository>()
            .AddScoped<IFuelTypesRepository, FuelTypesRepository>();
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

    private static IServiceCollection AddMessageBus(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly? assembly = null)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            if (assembly != null)
                x.AddConsumers(assembly);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
                cfg.Host(configuration["RabbitMQ:Host"], h =>
                {
                    h.Username(configuration["RabbitMQ:Username"]!);
                    h.Password(configuration["RabbitMQ:Password"]!);
                });
            });
        });

        return services;
    }
}