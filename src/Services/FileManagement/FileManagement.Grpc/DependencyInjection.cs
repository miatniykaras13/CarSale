using FileManagement.Grpc.Data;
using Microsoft.EntityFrameworkCore;
using Minio;

namespace FileManagement.Grpc;

public static class DependencyInjection
{
    public static void AddProgramDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpc();

        services.AddDbContext<FileManagementDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("FileManagementDbContext")));

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