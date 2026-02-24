using AdService.Application.Abstractions.Data;
using AdService.Domain.ValueObjects;
using BuildingBlocks.Messaging.Events.AutoCatalog;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdService.Infrastructure.MessageBus.EventHandlers;

public class BodyTypeUpdatedEventHandler(
    IAppDbContext dbContext,
    ILogger<BodyTypeUpdatedEventHandler> logger) : IConsumer<BodyTypeUpdatedEvent>
{
    public async Task Consume(ConsumeContext<BodyTypeUpdatedEvent> context)
    {
        logger.LogInformation("Received: {@event}", context.Message);
        var message = context.Message;

        var ads = await dbContext.Ads
            .Where(a => a.Car != null && a.Car.BodyType != null && a.Car.BodyType.Id == message.BodyTypeId)
            .ToListAsync();

        foreach (var ad in ads)
        {
            var bodyTypeResult = BodyTypeSnapshot.Of(message.BodyTypeId, message.BodyTypeName);

            if (bodyTypeResult.IsFailure)
            {
                logger.LogError(
                    "Error happened when creating {name} during handling {event}. Ad with id {adId} was not updated",
                    nameof(BodyTypeSnapshot),
                    message,
                    ad.Id);
                continue;
            }

            var newCarResult = CarSnapshot.Of(
                carId: ad.Car?.CarId,
                brand: ad.Car?.Brand,
                model: ad.Car?.Model,
                generation: ad.Car?.Generation,
                engine: ad.Car?.Engine,
                driveType: ad.Car?.DriveType,
                transmissionType: ad.Car?.TransmissionType,
                bodyType: bodyTypeResult.Value,
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