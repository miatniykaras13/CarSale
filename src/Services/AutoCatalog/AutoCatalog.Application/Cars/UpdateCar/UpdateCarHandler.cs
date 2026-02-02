using AutoCatalog.Application.Abstractions;
using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Application.Cars.Dtos;
using AutoCatalog.Application.Dtos;

namespace AutoCatalog.Application.Cars.UpdateCar;

public record UpdateCarCommand(
    Guid Id,
    int BrandId,
    int ModelId,
    int GenerationId,
    int EngineId,
    int BodyTypeId,
    int TransmissionTypeId,
    int DriveTypeId,
    float Consumption,
    float Acceleration,
    int FuelTankCapacity,
    DimensionsDto DimensionsDto) : ICommand<Result<Guid, List<Error>>>;

internal class UpdateCarCommandHandler(
    ICarsRepository carsRepository,
    IModelsRepository modelsRepository,
    IBrandsRepository brandsRepository,
    IEnginesRepository enginesRepository,
    IGenerationsRepository generationsRepository,
    IBodyTypesRepository bodyTypesRepository,
    ITransmissionTypesRepository transmissionTypesRepository,
    IAutoDriveTypesRepository driveTypesRepository) : ICommandHandler<UpdateCarCommand, Result<Guid, List<Error>>>
{
    public async Task<Result<Guid, List<Error>>> Handle(UpdateCarCommand command, CancellationToken cancellationToken)
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

        var brand = brandResult.Value;
        var model = modelResult.Value;
        var generation = generationResult.Value;
        var engine = engineResult.Value;

        if (brand.Id != command.BrandId)
        {
            return Result.Failure<Guid, List<Error>>(Error.Validation(
                "brand_id",
                "Cannot change the brand id. Recreate the car."));
        }

        if (model.Id != command.ModelId)
        {
            return Result.Failure<Guid, List<Error>>(Error.Validation(
                "model_id",
                "Cannot change the model id. Recreate the car."));
        }

        if (generation.Id != command.GenerationId)
        {
            return Result.Failure<Guid, List<Error>>(Error.Validation(
                "generation_id",
                "Cannot change the generation id. Recreate the car."));
        }

        if (engine.Id != command.EngineId)
        {
            return Result.Failure<Guid, List<Error>>(Error.Validation(
                "engine_id",
                "Cannot change the engine id. Recreate the car."));
        }

        var carResult = await carsRepository.GetByIdAsync(command.Id, cancellationToken);
        if (carResult.IsFailure)
            return Result.Failure<Guid, List<Error>>([carResult.Error]);
        var car = carResult.Value;

        command.Adapt(car);

        await carsRepository.UpdateAsync(car, cancellationToken);

        return Result.Success<Guid, List<Error>>(car.Id);
    }
}