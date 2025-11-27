using AdService.Application.FileStorage;
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
        });
        services.AddScoped<IFileStorage, MinioFileStorage>();
        return services;
    }
}