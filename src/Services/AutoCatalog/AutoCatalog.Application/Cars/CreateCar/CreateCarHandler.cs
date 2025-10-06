using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Enums;
using AutoCatalog.Domain.Specs;
using AutoCatalog.Domain.Transport.Cars;
using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using BuildingBlocks.Extensions;
using CSharpFunctionalExtensions;

namespace AutoCatalog.Application.Cars.CreateCar;

public record CreateCarCommand(
    int BrandId,
    int ModelId,
    int GenerationId,
    int EngineId,
    TransmissionType TransmissionType,
    AutoDriveType AutoDriveType,
    int YearFrom,
    int YearTo,
    Guid PhotoId,
    float Consumption,
    float Acceleration,
    int FuelTankCapacity,
    DimensionsDto DimensionsDto) : ICommand<Result<Guid, List<Error>>>;

internal class CreateCarCommandHandler(
    ICarsRepository carsRepository,
    IModelsRepository modelsRepository,
    IBrandsRepository brandsRepository,
    IEnginesRepository enginesRepository,
    IGenerationsRepository generationsRepository) : ICommandHandler<CreateCarCommand, Result<Guid, List<Error>>>
{
    public async Task<Result<Guid, List<Error>>> Handle(CreateCarCommand command, CancellationToken cancellationToken)
    {
        var brandResult = await brandsRepository.GetByIdAsync(command.BrandId, cancellationToken);
        if (brandResult.IsFailure)
        {
            return Result.Failure<Guid, List<Error>>([brandResult.Error]);
        }

        if (command.YearFrom < brandResult.Value.YearFrom || command.YearTo > brandResult.Value.YearTo)
            return Result.Failure<Guid, List<Error>>([Error.Validation("car", "Car year is invalid")]);

        var modelResult = await modelsRepository.GetByIdAsync(command.ModelId, cancellationToken);
        if (modelResult.IsFailure)
        {
            return Result.Failure<Guid, List<Error>>([modelResult.Error]);
        }

        var generationResult = await generationsRepository.GetByIdAsync(command.GenerationId, cancellationToken);
        if (generationResult.IsFailure)
        {
            return Result.Failure<Guid, List<Error>>([generationResult.Error]);
        }

        var engineResult = await enginesRepository.GetByIdAsync(command.EngineId, cancellationToken);
        if (engineResult.IsFailure)
        {
            return Result.Failure<Guid, List<Error>>([engineResult.Error]);
        }
        
        


        Car car = new()
        {
            Id = Guid.CreateVersion7(),
            BrandId = command.BrandId,
            Brand = brandResult.Value,
            ModelId = command.ModelId,
            Model = modelResult.Value,
            GenerationId = command.GenerationId,
            Generation = generationResult.Value,
            EngineId = command.EngineId,
            Engine = engineResult.Value,
            TransmissionType = command.TransmissionType,
            AutoDriveType = command.AutoDriveType,
            YearFrom = command.YearFrom,
            YearTo = command.YearTo,
            PhotoId = command.PhotoId,
            Consumption = command.Consumption,
            Acceleration = command.Acceleration,
            FuelTankCapacity = command.FuelTankCapacity,
            Dimensions = new Dimensions(
                command.DimensionsDto.Width,
                command.DimensionsDto.Height,
                command.DimensionsDto.Length),
        };

        await carsRepository.AddAsync(car, cancellationToken);

        return Result.Success<Guid, List<Error>>(car.Id);
    }
}