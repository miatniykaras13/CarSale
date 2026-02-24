using AdService.Application.Abstractions.Data;
using AdService.Domain.ValueObjects;
using BuildingBlocks.Messaging.Events.AutoCatalog;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdService.Infrastructure.MessageBus.EventHandlers;

public class EngineUpdatedEventHandler(
    IAppDbContext dbContext,
    ILogger<EngineUpdatedEventHandler> logger) : IConsumer<EngineUpdatedEvent>
{
    public async Task Consume(ConsumeContext<EngineUpdatedEvent> context)
    {
        logger.LogInformation("Received: {@event}", context.Message);
        var message = context.Message;

        var ads = await dbContext.Ads
            .Where(a => a.Car != null && a.Car.Engine != null && a.Car.Engine.Id == message.EngineId)
            .ToListAsync();

        foreach (var ad in ads)
        {
            var fuelTypeSnapshotResult = FuelTypeSnapshot.Of(message.FuelTypeId, message.FuelTypeName);

            if (fuelTypeSnapshotResult.IsFailure)
            {
                logger.LogError(
                    "Error happened when creating {name} during handling {event}. Ad with id {adId} was not updated",
                    nameof(FuelTypeSnapshot),
                    message,
                    ad.Id);
                continue;
            }

            var newEngineResult = EngineSnapshot.Of(
                message.EngineId,
                message.EngineName,
                message.HorsePower,
                fuelTypeSnapshotResult.Value,
                message.GenerationId);

            if (newEngineResult.IsFailure)
            {
                logger.LogError(
                    "Error happened when creating {name} during handling {event}. Ad with id {adId} was not updated",
                    nameof(EngineSnapshot),
                    message,
                    ad.Id);
                continue;
            }

            var newCarResult = CarSnapshot.Of(
                carId: ad.Car?.CarId,
                brand: ad.Car?.Brand,
                model: ad.Car?.Model,
                generation: ad.Car?.Generation,
                engine: newEngineResult.Value,
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