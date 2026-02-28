using AdService.Application.Commands.RemoveAdsFromDatabase;
using AdService.Application.Options;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AdService.Infrastructure.Core.BackgroundServices;

public class AdDeletionBackgroundService(
    IServiceScopeFactory scopeFactory,
    IOptions<AdDeletionOptions> options,
    ILogger<AdDeletionBackgroundService> logger) : BackgroundService
{
    private readonly AdDeletionOptions _deletionOptions = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("{Service} started.", nameof(AdDeletionBackgroundService));
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(new RemoveAdsFromDatabaseCommand(), stoppingToken);
            
            try
            {
                await Task.Delay(_deletionOptions.CheckInterval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("{Service} stopping.", nameof(AdDeletionBackgroundService));
            }
        }
    }
}