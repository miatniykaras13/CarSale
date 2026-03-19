using AdService.Application.Abstractions.Data;
using AdService.Domain.ValueObjects;
using BuildingBlocks.Messaging.Events;
using BuildingBlocks.Messaging.Events.AutoCatalog;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdService.Infrastructure.MessageBus.EventHandlers;

public class BrandUpdatedEventHandler(
    IAppDbContext dbContext,
    ILogger<BrandUpdatedEventHandler> logger) : IConsumer<BrandUpdatedEvent>
{
    public async Task Consume(ConsumeContext<BrandUpdatedEvent> context)
    {
        logger.LogInformation("Received brand updated event: {@event}", context.Message);
        var message = context.Message;

        var ads = await dbContext.Ads
            .Where(a => a.Car != null && a.Car.Brand != null && a.Car.Brand.Id == message.BrandId)
            .ToListAsync();

        foreach (var ad in ads)
        {
            var newBrandResult = BrandSnapshot.Of(message.BrandId, message.BrandName);
            if (newBrandResult.IsFailure)
            {
                logger.LogError(
                    "Error happened when creating {name} during handling {event}. Ad with id {adId} was not updated",
                    nameof(BrandSnapshot),
                    message,
                    ad.Id);
                continue;
            }

            var newCarResult = CarSnapshot.Of(
                carId: ad.Car?.CarId,
                brand: newBrandResult.Value,
                model: ad.Car?.Model,
                generation: ad.Car?.Generation,
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