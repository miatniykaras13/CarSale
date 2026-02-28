using AdService.Application.Commands.ExpireAds;
using AdService.Application.Options;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AdService.Infrastructure.Core.BackgroundServices;

public class AdExpirationCheckerService(
    IServiceScopeFactory factory,
    IOptions<AdExpirationOptions> options,
    ILogger<AdExpirationCheckerService> logger) : BackgroundService
{
    private readonly AdExpirationOptions _options = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("{Service} started.", nameof(AdExpirationCheckerService));
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = factory.CreateScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                var result = await sender.Send(new ExpireAdsCommand(), stoppingToken);
                if (result.IsFailure)
                {
                    logger.LogWarning("Ads expiration went with errors.");
                }
                else
                {
                    logger.LogInformation("Ads expiration check completed.");
                }

                await Task.Delay(_options.CheckInterval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("{Service} stopping.", nameof(AdExpirationCheckerService));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}