using FileManagement.Grpc.Data;
using FileManagement.Grpc.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using FileInfo = FileManagement.Grpc.Models.FileInfo;

namespace FileManagement.Grpc.BackgroundServices;

public class MinioSyncService(
    IMinioClient minio,
    IServiceScopeFactory scopeFactory,
    IOptions<MinioSyncOptions> options,
    ILogger<MinioSyncService> logger) : BackgroundService
{
    private readonly MinioSyncOptions _syncOptions = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Даём время другим сервисам (MinIO) запуститься
        await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SynchronizeAllBuckets(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during MinIO synchronization");
            }

            await Task.Delay(_syncOptions.SyncInterval, stoppingToken);
        }
    }

    private async Task SynchronizeAllBuckets(CancellationToken ct)
    {
        var buckets = await minio.ListBucketsAsync(ct);

        foreach (var bucket in buckets.Buckets)
        {
            logger.LogInformation("Synchronizing bucket {BucketName}", bucket.Name);
            await SynchronizeBucket(bucket.Name, ct);
        }
    }

    private async Task SynchronizeBucket(string bucketName, CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FileManagementDbContext>();

        var existingFileIds = await dbContext.Files
            .Where(f => f.BucketName == bucketName)
            .Select(f => f.Id)
            .ToListAsync(ct);

        var listArgs = new ListObjectsArgs()
            .WithBucket(bucketName)
            .WithRecursive(true);

        var objects = minio.ListObjectsEnumAsync(listArgs, ct);

        await foreach (var item in objects)
        {
            if (item.IsDir)
                continue;

            if (existingFileIds.Any(id => item.Key.Contains(id.ToString())))
                continue;

            try
            {
                var statArgs = new StatObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(item.Key);
                var stat = await minio.StatObjectAsync(statArgs, ct);

                var metadata = stat.ExtraHeaders;

                metadata.TryGetValue("x-file-name", out var fileNameHeader);
                var fileName = fileNameHeader ?? Path.GetFileName(item.Key);

                metadata.TryGetValue("x-file-width", out var widthHeader);
                var fileWidth = int.TryParse(widthHeader, out var w) ? w : 0;

                metadata.TryGetValue("x-file-height", out var heightHeader);
                var fileHeight = int.TryParse(heightHeader, out var h) ? h : 0;

                Guid? parentId = null;
                if (metadata.TryGetValue("x-parent-id", out var parentIdHeader)
                    && Guid.TryParse(parentIdHeader, out var parsedParentId)
                    && parsedParentId != Guid.Empty)
                {
                    parentId = parsedParentId;
                }

                var fileId = Guid.Parse(Path.GetFileName(item.Key).Split('_').First());

                var fileInfo = FileInfo.Create(
                    fileId,
                    fileName,
                    (long)item.Size,
                    Path.GetExtension(fileName),
                    item.LastModifiedDateTime ?? DateTime.UtcNow,
                    stat.ContentType,
                    bucketName,
                    fileWidth,
                    fileHeight,
                    parentId);

                await dbContext.Files.AddAsync(fileInfo, ct);
                logger.LogInformation("Synced file {ObjectKey} to DB", item.Key);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to sync object {ObjectKey}", item.Key);
            }
        }

        await dbContext.SaveChangesAsync(ct);
    }
}