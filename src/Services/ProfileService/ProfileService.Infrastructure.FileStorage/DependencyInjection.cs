using FileManagement.Grpc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProfileService.Application.Abstractions.FileStorage;
using ProfileService.Application.Options;

namespace ProfileService.Infrastructure.FileStorage;

public static class DependencyInjection
{
    public static IServiceCollection AddFileStorage(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<FileStorageOptions>(configuration.GetSection("FileStorage"));

        services.AddGrpcClient<FileManager.FileManagerClient>(options =>
            {
                options.Address = new Uri(configuration["FileStorage:Endpoint"] ??
                    throw new InvalidOperationException("FileStorage:Endpoint is not configured."));
            })
            .ConfigureChannel(options =>
            {
                options.MaxReceiveMessageSize = null;
                options.MaxSendMessageSize = null;
            })
            .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
            {
                var handler = new SocketsHttpHandler
                {
                    EnableMultipleHttp2Connections = true,
                };

                if (serviceProvider.GetRequiredService<IHostEnvironment>().IsDevelopment())
                {
                    // Local and Docker development use the shared self-signed ASP.NET certificate.
                    #pragma warning disable CA5359
                    handler.SslOptions.RemoteCertificateValidationCallback = (_, _, _, _) => true;
                    #pragma warning restore CA5359
                }

                return handler;
            });

        services.AddScoped<IFileStorage, MinioFileStorage>();
        return services;
    }
}