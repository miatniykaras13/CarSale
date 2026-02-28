namespace AdService.Application.Options;

public class CacheOptions
{
    public TimeSpan? DefaultMemoryAbsoluteExpiration { get; set; }

    public TimeSpan? DefaultDistributedAbsoluteExpiration { get; set; }

    public TimeSpan? AdListItemAbsoluteExpiration { get; set; }

    public TimeSpan? AdIndexAbsoluteExpiration { get; set; }

    public TimeSpan? AutoCatalogEntityAbsoluteExpiration { get; set; }

    public TimeSpan? CarOptionAbsoluteExpiration { get; set; }

    public TimeSpan? CarOptionIndexAbsoluteExpiration { get; set; }
}