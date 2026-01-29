namespace AdService.Infrastructure.Redis.Options;

public class CacheOptions
{
    public TimeSpan? MemoryAbsoluteExpiration { get; set; }

    public TimeSpan? DistributedAbsoluteExpiration { get; set; }
}