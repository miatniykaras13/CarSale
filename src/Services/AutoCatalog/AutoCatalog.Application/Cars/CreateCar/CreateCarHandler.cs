using AutoCatalog.Application.Abstractions;
using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Application.Cars.Dtos;
using AutoCatalog.Application.Dtos;
using AutoCatalog.Domain.Specs;
using AutoCatalog.Domain.Transport.Cars;

namespace AutoCatalog.Application.Cars.CreateCar;

public record CreateCarCommand(
    int BrandId,
    int ModelId,
    int GenerationId,
    int EngineId,
    int TransmissionTypeId,
    int DriveTypeId,
    int BodyTypeId,
    float Consumption,
    float Acceleration,
    int FuelTankCapacity,
    DimensionsDto DimensionsDto) : ICommand<Result<Guid, List<Error>>>;

internal class CreateCarCommandHandler(
    ICarsRepository carsRepository,
    IModelsRepository modelsRepository,
    IBrandsRepository brandsRepository,
    IEnginesRepository enginesRepository,
    IGenerationsRepository generationsRepository,
    IBodyTypesRepository bodyTypesRepository,
    ITransmissionTypesRepository transmissionTypesRepository,
    IAutoDriveTypesRepository driveTypesRepository) : ICommandHandler<CreateCarCommand, Result<Guid, List<Error>>>
{
    public async Task<Result<Guid, List<Error>>> Handle(CreateCarCommand command, CancellationToken cancellationToken)
    {
        var brandTask = brandsRepository.GetByIdAsync(command.BrandId, cancellationToken);
        var modelTask = modelsRepository.GetByIdAsync(command.ModelId, cancellationToken);
        var generationTask = generationsRepository.GetByIdAsync(command.GenerationId, cancellationToken);
        var engineTask = enginesRepository.GetByIdAsync(command.EngineId, cancellationToken);
        var transmissionTypeTask =
            transmissionTypesRepository.GetByIdAsync(command.TransmissionTypeId, cancellationToken);
        var driveTypeTask = driveTypesRepository.GetByIdAsync(command.DriveTypeId, cancellationToken);
        var bodyTypeTask = bodyTypesRepository.GetByIdAsync(command.BodyTypeId, cancellationToken);

        await Task.WhenAll(
            brandTask,
            bodyTypeTask,
            driveTypeTask,
            engineTask,
            generationTask,
            transmissionTypeTask,
            modelTask);

        var brandResult = await brandTask;
        var modelResult = await modelTask;
        var generationResult = await generationTask;
        var engineResult = await engineTask;
        var transmissionTypeResult = await transmissionTypeTask;
        var driveTypeResult = await driveTypeTask;
        var bodyTypeResult = await bodyTypeTask;


        if (brandResult.IsFailure)
            return Result.Failure<Guid, List<Error>>([brandResult.Error]);

        if (modelResult.IsFailure)
            return Result.Failure<Guid, List<Error>>([modelResult.Error]);

        if (generationResult.IsFailure)
            return Result.Failure<Guid, List<Error>>([generationResult.Error]);

        if (engineResult.IsFailure)
            return Result.Failure<Guid, List<Error>>([engineResult.Error]);

        if (transmissionTypeResult.IsFailure)
            return Result.Failure<Guid, List<Error>>([transmissionTypeResult.Error]);

        if (driveTypeResult.IsFailure)
            return Result.Failure<Guid, List<Error>>([driveTypeResult.Error]);

        if (bodyTypeResult.IsFailure)
            return Result.Failure<Guid, List<Error>>([bodyTypeResult.Error]);


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
            TransmissionTypeId = command.TransmissionTypeId,
            TransmissionType = transmissionTypeResult.Value,
            DriveTypeId = command.DriveTypeId,
            DriveType = driveTypeResult.Value,
            BodyTypeId = command.BodyTypeId,
            BodyType = bodyTypeResult.Value,
            PhotoId = null,
            Consumption = command.Consumption,
            Acceleration = command.Acceleration,
            FuelTankCapacity = command.FuelTankCapacity,
            Dimensions = new Dimensions()
            {
                Width = command.DimensionsDto.Width,
                Height = command.DimensionsDto.Height,
                Length = command.DimensionsDto.Length,
            },
        };

        await carsRepository.AddAsync(car, cancellationToken);
        return Result.Success<Guid, List<Error>>(car.Id);
    }
}