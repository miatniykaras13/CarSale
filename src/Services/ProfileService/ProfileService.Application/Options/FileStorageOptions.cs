namespace ProfileService.Application.Options;

public class FileStorageOptions
{
    public long MaxFileSize { get; set; } = 10 * 1024 * 1024;

    public int DownloadLinkExpirySeconds { get; set; } = 600;
}