namespace AdService.Application.Options;

public class AdDeletionOptions
{
    public TimeSpan CheckInterval { get; set; }

    public TimeSpan TimeToRestore { get; set; }
}