using AdService.Application.Abstractions.Data;
using AdService.Domain.ValueObjects;
using BuildingBlocks.Messaging.Events.AutoCatalog;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdService.Infrastructure.MessageBus.EventHandlers;

public class GenerationUpdatedEventHandler(
    IAppDbContext dbContext,
    ILogger<GenerationUpdatedEventHandler> logger) : IConsumer<GenerationUpdatedEvent>
{
    public async Task Consume(ConsumeContext<GenerationUpdatedEvent> context)
    {
        logger.LogInformation("Received: {@event}", context.Message);
        var message = context.Message;

        var ads = await dbContext.Ads
            .Where(a => a.Car != null && a.Car.Generation != null && a.Car.Generation.Id == message.GenerationId)
            .ToListAsync();

        foreach (var ad in ads)
        {
            var newGenerationResult = GenerationSnapshot.Of(
                message.GenerationId,
                message.GenerationName,
                message.ModelId,
                message.YearFrom,
                message.YearTo);
            if (newGenerationResult.IsFailure)
            {
                logger.LogError(
                    "Error happened when creating {name} during handling {event}. Ad with id {adId} was not updated",
                    nameof(GenerationSnapshot),
                    message,
                    ad.Id);
                continue;
            }

            var newCarResult = CarSnapshot.Of(
                carId: ad.Car?.CarId,
                brand: ad.Car?.Brand,
                model: ad.Car?.Model,
                generation: newGenerationResult.Value,
                engine: ad.Car?.Engine,
                driveType: ad.Car?.DriveType,
                transmissionType: ad.Car?.TransmissionType,
                bodyType: ad.Car?.BodyType,
                year: ad.Car?.Year,
                vin: ad.Car?.Vin,
                mileage: ad.Car?.Mileage,
                consumption: ad.Car?.Consumption,
                color: ad.Car?.Color);

            if (newCarResult.IsFailure)
            {
                logger.LogError(
                    "Error happened when creating {name} during handling {event}. Ad with id {adId} was not updated",
                    nameof(CarSnapshot),
                    message,
                    ad.Id);
                continue;
            }

            var updateResult = ad.UpdateCar(newCarResult.Value);
            if (updateResult.IsFailure)
            {
                logger.LogError(
                    "Error happened when updating ad with id {name} during handling {event}.",
                    ad.Id,
                    message);
            }
        }

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}