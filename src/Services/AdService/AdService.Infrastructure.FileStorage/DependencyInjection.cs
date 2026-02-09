using System.Net.Security;
using AdService.Application.Abstractions.FileStorage;
using FileManagement.Grpc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdService.Infrastructure.FileStorage;

public static class DependencyInjection
{
    public static IServiceCollection AddFileStorage(this IServiceCollection services, IConfiguration configuration)
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