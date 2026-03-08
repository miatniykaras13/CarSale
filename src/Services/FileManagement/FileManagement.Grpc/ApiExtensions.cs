using FileManagement.Grpc.Data;
using FileManagement.Grpc.Infra;
using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel.Args;
using FileInfo = FileManagement.Grpc.Models.FileInfo;

namespace FileManagement.Grpc;

public static class ApiExtensions
{
    private static readonly Guid[] AdServiceSeedGuids =
    [
        Guid.Parse("1c5cd26e-64ae-4557-86ce-6bc4a393e5a0"),
        Guid.Parse("aa39432f-08a0-4dda-895f-05307f159976"),
        Guid.Parse("7dea9638-2d79-44d8-8bb1-5edb606b207d"),
        Guid.Parse("3288e3d6-fee5-4548-bebd-04417729909d"),
    ];

    private static readonly Guid[] AutoCatalogSeedGuids =
    [
        Guid.Parse("67a7faa1-f840-4c39-b775-a8c736f575b6"),
        Guid.Parse("3b44e9ce-a0ca-4330-b4ee-260ebf5cd07c"),
        Guid.Parse("fc2e73e0-46ab-4384-a093-ba5f00d20fdf"),
        Guid.Parse("a61302ad-1968-4892-a820-3aebae57ebbd"),
    ];

    public static async Task<WebApplication> UseMigrating(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<FileManagementDbContext>();
        await context.Database.EnsureCreatedAsync();
        return app;
    }

    public static async Task<WebApplication> UseSeedingAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<FileManagementDbContext>();
        var minio = scope.ServiceProvider.GetRequiredService<IMinioClient>();
        var imageProcessor = scope.ServiceProvider.GetRequiredService<IImageProcessor>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<FileManagementDbContext>>();

        if (await context.Files.AnyAsync())
            return app;

        const string adServiceBucketName = "ad-service";
        const string autoCatalogBucketName = "auto-catalog";

        // Ensure buckets exist
        bool adServiceBucketExists =
            await minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(adServiceBucketName));
        bool autoCatalogBucketExists =
            await minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(autoCatalogBucketName));

        if (!adServiceBucketExists)
        {
            logger.LogWarning("Bucket '{BucketName}' does not exist in MinIO. Skipping seeding.", adServiceBucketName);
            return app;
        }

        if (!autoCatalogBucketExists)
        {
            logger.LogWarning(
                "Bucket '{BucketName}' does not exist in MinIO. Skipping seeding.",
                autoCatalogBucketName);
            return app;
        }

        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            await SeedBucketAsync(
                "Data/Initial/Images/AdService",
                adServiceBucketName,
                AdServiceSeedGuids,
                context,
                minio,
                imageProcessor,
                logger);

            await SeedBucketAsync(
                "Data/Initial/Images/AutoCatalog",
                autoCatalogBucketName,
                AutoCatalogSeedGuids,
                context,
                minio,
                imageProcessor,
                logger);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            logger.LogInformation("Successfully seeded files into ad-service and auto-catalog buckets");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "Failed to seed files. Transaction rolled back.");
            throw;
        }

        return app;
    }

    private static async Task SeedBucketAsync(
        string imageDirectoryPath,
        string bucketName,
        Guid[] seedGuids,
        FileManagementDbContext context,
        IMinioClient minio,
        IImageProcessor imageProcessor,
        ILogger logger)
    {
        var directoryInfo = new DirectoryInfo(imageDirectoryPath);

        if (!directoryInfo.Exists)
        {
            logger.LogWarning("Seed images directory not found: {Path}", directoryInfo.FullName);
            return;
        }

        var photosToSeed = directoryInfo.GetFiles();

        if (photosToSeed.Length == 0)
        {
            logger.LogWarning("No seed images found in {Path}", directoryInfo.FullName);
            return;
        }

        var count = Math.Min(photosToSeed.Length, seedGuids.Length);

        for (int i = 0; i < count; i++)
        {
            await using var stream = photosToSeed[i].OpenRead();

            var (photoWidth, photoHeight) = imageProcessor.GetImageSize(stream);
            stream.Position = 0;

            var contentType = DefineContentTypeByExtension(photosToSeed[i].Extension);

            var fileInfo = FileInfo.Create(
                seedGuids[i],
                Path.GetFileNameWithoutExtension(photosToSeed[i].Name),
                stream.Length,
                photosToSeed[i].Extension,
                DateTime.UtcNow,
                contentType,
                bucketName,
                photoWidth,
                photoHeight);

            var objectName = GenerateSeedObjectName(
                photosToSeed[i].Name, contentType, fileInfo.Id, fileInfo.Size);

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(contentType);

            await minio.PutObjectAsync(putObjectArgs);

            await context.Files.AddAsync(fileInfo);

            logger.LogInformation(
                "Seeded file {FileName} with ID {FileId} into bucket {BucketName}",
                photosToSeed[i].Name,
                seedGuids[i],
                bucketName);
        }
    }

    private static string DefineContentTypeByExtension(string? extension) =>
        extension?.ToLowerInvariant() switch
        {
            // Изображения
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",

            // Документы
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".txt" => "text/plain",
            ".csv" => "text/csv",

            // Аудио
            ".mp3" => "audio/mpeg",
            ".wav" => "audio/wav",
            ".ogg" => "audio/ogg",

            // Видео
            ".mp4" => "video/mp4",
            ".webm" => "video/webm",
            ".avi" => "video/x-msvideo",

            // По умолчанию
            _ => "application/octet-stream"
        };

    private static string DefineFolderByContentType(string? contentType) =>
        contentType switch
        {
            "image/jpeg" or "image/png" or "image/gif" or "image/webp" => "images",
            "application/pdf" or "application/msword"
                or "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                or "application/vnd.ms-excel"
                or "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                or "text/plain" or "text/csv" => "documents",
            "audio/mpeg" or "audio/wav" or "audio/ogg" => "audio",
            "video/mp4" or "video/webm" or "video/x-msvideo" => "video",
            _ => "misc"
        };

    private static string GenerateSeedObjectName(string? fileName, string contentType, Guid fileId, long fileSize) =>
        $"{DefineFolderByContentType(contentType)}/{fileId}_{fileSize}{Path.GetExtension(fileName)}";
}