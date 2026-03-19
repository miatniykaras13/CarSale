using AutoCatalog.Application.Abstractions;
using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Application.Cars.Dtos;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Messaging.Events;
using BuildingBlocks.Messaging.Events.AutoCatalog;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AutoCatalog.Application.Brands.UpdateBrand;

public record UpdateBrandCommand(int Id, string Name, string Country, int YearFrom, int? YearTo)
    : ICommand<Result<int, List<Error>>>;

internal class UpdateBrandCommandHandler(
    IBrandsRepository brandsRepository,
    IPublishEndpoint publishEndpoint,
    ILogger<UpdateBrandCommandHandler> logger)
    : ICommandHandler<UpdateBrandCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(UpdateBrandCommand command, CancellationToken cancellationToken)
    {
        var brandResult = await brandsRepository.GetByIdAsync(command.Id, cancellationToken);
        if (brandResult.IsFailure)
            return Result.Failure<int, List<Error>>([brandResult.Error]);

        var brand = brandResult.Value;

        if (!brand.Name.Equals(command.Name, StringComparison.OrdinalIgnoreCase))
        {
            var brandUpdatedEvent = new BrandUpdatedEvent
            {
                BrandId = command.Id,
                BrandName = command.Name,
                Country = command.Country,
                YearFrom = command.YearFrom,
                YearTo = command.YearTo,
            };
            logger.LogInformation("Publishing event {EventName}", brandUpdatedEvent);
            await publishEndpoint.Publish(brandUpdatedEvent, cancellationToken);
        }

        command.Adapt(brand);

        await brandsRepository.UpdateAsync(brand, cancellationToken);

        return Result.Success<int, List<Error>>(command.Id);
    }
}