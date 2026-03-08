using FileManagement.Grpc.BackgroundServices;
using FileManagement.Grpc.Data;
using FileManagement.Grpc.Infra;
using FileManagement.Grpc.Options;
using Microsoft.EntityFrameworkCore;
using Minio;

namespace FileManagement.Grpc;

public static class DependencyInjection
{
    public static void AddProgramDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpc(options =>
        {
            options.MaxReceiveMessageSize = null;
            options.MaxSendMessageSize = null;
        });

        services.AddDbContext<FileManagementDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("FileManagementDbContext")));

        services.Configure<MinioSyncOptions>(configuration.GetSection("Minio"));
        services.AddScoped<IImageProcessor, ImageProcessor>();

        services.AddHostedService<MinioSyncService>();

        services.AddFileStorage(configuration);
    }

    private static void AddFileStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var minioConfig = configuration.GetSection("Minio");

        var endpoint = minioConfig["Endpoint"]!;
        var accessKey = minioConfig["AccessKey"]!;
        var secretKey = minioConfig["SecretKey"]!;
        var useSSL = bool.Parse(minioConfig["UseSSL"]!);

        services.AddMinio(configureClient =>
            configureClient
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .WithSSL(useSSL)
                .Build());
    }
}