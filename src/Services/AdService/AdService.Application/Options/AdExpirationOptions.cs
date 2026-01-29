namespace AdService.Application.Options;

public class AdExpirationOptions
{
    public TimeSpan CheckInterval { get; set; }

    public TimeSpan AdLifeSpan { get; set; }
}