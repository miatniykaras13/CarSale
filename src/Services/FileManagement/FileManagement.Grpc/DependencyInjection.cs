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

        services.AddServiceHealthChecks(configuration);
    }

    private static void AddFileStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var minioConfig = configuration.GetSection("Minio");

        var endpoint = minioConfig["Endpoint"]!;
        var accessKey = minioConfig["AccessKey"]!;
        var secretKey = minioConfig["SecretKey"]!;
        var useSSL = bool.Parse(minioConfig["UseSSL"]!);
        var publicEndpoint = minioConfig["PublicEndpoint"] ?? endpoint;
        var publicUseSSL = minioConfig.GetValue<bool?>("PublicUseSSL") ?? useSSL;

        services.AddMinio(configureClient =>
            configureClient
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .WithSSL(useSSL)
                .Build());

        services.AddKeyedSingleton<IMinioClient>("public", (_, _) =>
            new MinioClient()
                .WithEndpoint(publicEndpoint)
                .WithCredentials(accessKey, secretKey)
                .WithSSL(publicUseSSL)
                .Build());
    }

    private static IServiceCollection AddServiceHealthChecks(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddHealthChecks()
            .AddSqlite(
                connectionString: configuration.GetConnectionString(nameof(FileManagementDbContext)) ??
                                  throw new InvalidOperationException(
                                      $"Connection string for '{nameof(FileManagementDbContext)}' is not configured."),
                name: "sqlite",
                tags: ["ready", "db"]);
        return services;
    }
}