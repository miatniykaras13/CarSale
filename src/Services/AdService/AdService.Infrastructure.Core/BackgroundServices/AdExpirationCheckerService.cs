using AdService.Application.Commands.ExpireAds;
using AdService.Infrastructure.Postgres.Data;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AdService.Infrastructure.Core.BackgroundServices;

public class AdExpirationCheckerService(
    IServiceScopeFactory factory,
    IConfiguration configuration,
    ILogger<AdExpirationCheckerService> logger) : BackgroundService
{
    private readonly TimeSpan _interval = configuration.GetValue<TimeSpan>(
        "AdExpiration:CheckInterval",
        TimeSpan.FromMinutes(10));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("{Service} started.", nameof(AdExpirationCheckerService));
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = factory.CreateScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                var response = await sender.Send(new ExpireAdsCommand(), stoppingToken);
                if (response.Result.IsFailure)
                {
                    logger.LogWarning("Ads expiration went with errors.");
                }
                else
                {
                    logger.LogInformation("Ads expiration check completed.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            try
            {
                await Task.Delay(_interval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("{Service} stopping.",  nameof(AdExpirationCheckerService));
            }
        }
    }
}