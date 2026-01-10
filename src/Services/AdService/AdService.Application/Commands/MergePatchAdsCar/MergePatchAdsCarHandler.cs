using System.Text.Json;
using AdService.Application.Abstractions.AutoCatalog;
using AdService.Application.Abstractions.Data;
using AdService.Application.Abstractions.Helpers;
using AdService.Contracts.Ads.MergePatch;
using AdService.Contracts.AutoCatalog;
using AdService.Domain.ValueObjects;

namespace AdService.Application.Commands.MergePatchAdsCar;

public class MergePatchAdsCarCommandHandler(
    IAppDbContext dbContext,
    IAutoCatalogClient autoCatalog,
    IMergePatchHelper mergePatchHelper)
    : ICommandHandler<MergePatchAdsCarCommand, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(MergePatchAdsCarCommand command, CancellationToken ct)
    {
        var ad = await dbContext.Ads.FindAsync([command.AdId], ct);

        if (ad is null)
            return UnitResult.Failure(Error.NotFound("ad", $"Ad with id {command.AdId} not found"));

        var patch = command.Patch;

        var currentCar = ad.Car;

        var currentCarDto = currentCar is not null
            ? new CarSnapshotDto()
            {
                BrandId = currentCar.Brand?.Id,
                ModelId = currentCar.Model?.Id,
                GenerationId = currentCar.Generation?.Id,
                EngineId = currentCar.Engine?.Id,
                DriveTypeId = currentCar.DriveType?.Id,
                TransmissionTypeId = currentCar.TransmissionType?.Id,
                BodyTypeId = currentCar.BodyType?.Id,
                Vin = currentCar.Vin,
                Color = currentCar.Color,
                Consumption = currentCar.Consumption,
                Year = currentCar.Year,
                Mileage = currentCar.Mileage,
            }
            : new CarSnapshotDto();

        var patchedDto = mergePatchHelper.ApplyMergePatch(currentCarDto, patch);


        if (patchedDto.ModelId is not null && patchedDto.BrandId is null)
        {
            return UnitResult.Failure(Error.Validation(
                "patched_car.model_id",
                "Brand id is required when model id is provided"));
        }

        if (patchedDto.GenerationId is not null && patchedDto.ModelId is null)
        {
            return UnitResult.Failure(Error.Validation(
                "patched_car.generation_id",
                "Model id is required when generation id is provided"));
        }

        if (patchedDto.EngineId is not null && patchedDto.GenerationId is null)
        {
            return UnitResult.Failure(Error.Validation(
                "patched_car.engine_id",
                "Generation id is required when engine id is provided"));
        }

        if (patchedDto.BodyTypeId is not null && patchedDto.EngineId is null)
        {
            return UnitResult.Failure(Error.Validation(
                "patched_car.body_type_id",
                "Engine id is required when body type id is provided"));
        }

        if (patchedDto.DriveTypeId is not null && patchedDto.EngineId is null)
        {
            return UnitResult.Failure(Error.Validation(
                "patched_car.drive_type_id",
                "Engine id is required when drive type id is provided"));
        }

        if (patchedDto.TransmissionTypeId is not null && patchedDto.EngineId is null)
        {
            return UnitResult.Failure(Error.Validation(
                "patched_car.transmission_type_id",
                "Engine id is required when transmission type id is provided"));
        }


        bool brandChanged = patchedDto.BrandId != currentCarDto.BrandId;
        bool modelChanged = patchedDto.ModelId != currentCarDto.ModelId;
        bool generationChanged = patchedDto.GenerationId != currentCarDto.GenerationId;
        bool engineChanged = patchedDto.EngineId != currentCarDto.EngineId;
        bool driveChanged = patchedDto.DriveTypeId != currentCarDto.DriveTypeId;
        bool bodyChanged = patchedDto.BodyTypeId != currentCarDto.BodyTypeId;
        bool transmissionChanged = patchedDto.TransmissionTypeId != currentCarDto.TransmissionTypeId;


        Task<Result<BrandDto, Error>>? brandTask = null;
        Task<Result<ModelDto, Error>>? modelTask = null;
        Task<Result<GenerationDto, Error>>? generationTask = null;
        Task<Result<EngineDto, Error>>? engineTask = null;
        Task<Result<TransmissionTypeDto, Error>>? transmissionTask = null;
        Task<Result<AutoDriveTypeDto, Error>>? driveTask = null;
        Task<Result<BodyTypeDto, Error>>? bodyTask = null;

        if (brandChanged && patchedDto.BrandId is not null)
            brandTask = autoCatalog.GetBrandByIdAsync(patchedDto.BrandId.Value, ct);

        if (modelChanged && patchedDto.ModelId is not null)
            modelTask = autoCatalog.GetModelByIdAsync(patchedDto.ModelId.Value, ct);

        if (generationChanged && patchedDto.GenerationId is not null)
            generationTask = autoCatalog.GetGenerationByIdAsync(patchedDto.GenerationId.Value, ct);

        if (engineChanged && patchedDto.EngineId is not null)
            engineTask = autoCatalog.GetEngineByIdAsync(patchedDto.EngineId.Value, ct);

        if (transmissionChanged && patchedDto.TransmissionTypeId is not null)
            transmissionTask = autoCatalog.GetTransmissionTypeByIdAsync(patchedDto.TransmissionTypeId.Value, ct);

        if (driveChanged && patchedDto.DriveTypeId is not null)
            driveTask = autoCatalog.GetAutoDriveTypeByIdAsync(patchedDto.DriveTypeId.Value, ct);

        if (bodyChanged && patchedDto.BodyTypeId is not null)
            bodyTask = autoCatalog.GetBodyTypeByIdAsync(patchedDto.BodyTypeId.Value, ct);


        var tasks = new List<Task>();
        if (brandTask is not null) tasks.Add(brandTask);
        if (modelTask is not null) tasks.Add(modelTask);
        if (generationTask is not null) tasks.Add(generationTask);
        if (engineTask is not null) tasks.Add(engineTask);
        if (transmissionTask is not null) tasks.Add(transmissionTask);
        if (driveTask is not null) tasks.Add(driveTask);
        if (bodyTask is not null) tasks.Add(bodyTask);

        if (tasks.Count > 0)
            await Task.WhenAll(tasks);

        BrandDto? brandDto = patchedDto.BrandId is not null && currentCar?.Brand is not null
            ? new BrandDto(currentCar.Brand.Id, currentCar.Brand.Name)
            : null;

        ModelDto? modelDto = patchedDto.ModelId is not null && currentCar?.Model is not null
            ? new ModelDto(currentCar.Model.Id, currentCar.Model.Name, currentCar.Model.BrandId)
            : null;

        GenerationDto? generationDto = patchedDto.GenerationId is not null && currentCar?.Generation is not null
            ? new GenerationDto(
                currentCar.Generation.Id,
                currentCar.Generation.Name,
                currentCar.Generation.ModelId,
                currentCar.Generation.YearFrom,
                currentCar.Generation.YearTo)
            : null;

        EngineDto? engineDto = patchedDto.EngineId is not null && currentCar?.Engine is not null
            ? new EngineDto(
                currentCar.Engine.Id,
                currentCar.Engine.GenerationId,
                currentCar.Engine.Name,
                new FuelTypeDto(currentCar.Engine.FuelType.Id, currentCar.Engine.FuelType.Name),
                0.0f,
                currentCar.Engine.HorsePower,
                0)
            : null;

        TransmissionTypeDto? transmissionDto =
            patchedDto.TransmissionTypeId is not null && currentCar?.TransmissionType is not null
                ? new TransmissionTypeDto(currentCar.TransmissionType.Id, currentCar.TransmissionType.Name)
                : null;

        AutoDriveTypeDto? driveDto = patchedDto.DriveTypeId is not null && currentCar?.DriveType is not null
            ? new AutoDriveTypeDto(currentCar.DriveType.Id, currentCar.DriveType.Name)
            : null;

        BodyTypeDto? bodyDto = patchedDto.BodyTypeId is not null && currentCar?.BodyType is not null
            ? new BodyTypeDto(currentCar.BodyType.Id, currentCar.BodyType.Name)
            : null;

        if (brandTask is not null)
        {
            var brandResult = await brandTask;
            if (brandResult.IsFailure) return UnitResult.Failure(brandResult.Error);
            brandDto = brandResult.Value;
        }

        if (modelTask is not null)
        {
            var modelResult = await modelTask;
            if (modelResult.IsFailure) return UnitResult.Failure(modelResult.Error);
            modelDto = modelResult.Value;
        }

        if (generationTask is not null)
        {
            var generationResult = await generationTask;
            if (generationResult.IsFailure) return UnitResult.Failure(generationResult.Error);
            generationDto = generationResult.Value;
        }

        if (engineTask is not null)
        {
            var engineResult = await engineTask;
            if (engineResult.IsFailure) return UnitResult.Failure(engineResult.Error);
            engineDto = engineResult.Value;
        }

        if (driveTask is not null)
        {
            var driveResult = await driveTask;
            if (driveResult.IsFailure) return UnitResult.Failure(driveResult.Error);
            driveDto = driveResult.Value;
        }

        if (transmissionTask is not null)
        {
            var transmissionResult = await transmissionTask;
            if (transmissionResult.IsFailure) return UnitResult.Failure(transmissionResult.Error);
            transmissionDto = transmissionResult.Value;
        }

        if (bodyTask is not null)
        {
            var bodyResult = await bodyTask;
            if (bodyResult.IsFailure) return UnitResult.Failure(bodyResult.Error);
            bodyDto = bodyResult.Value;
        }

        if (modelDto is not null && brandDto is not null && modelDto.BrandId != brandDto.Id)
            return UnitResult.Failure(Error.Validation("car_snapshot.model", "Model does not belong to Brand"));

        if (generationDto is not null && modelDto is not null && generationDto.ModelId != modelDto.Id)
        {
            return UnitResult.Failure(
                Error.Validation("car_snapshot.generation", "Generation does not belong to Model"));
        }

        if (engineDto is not null && generationDto is not null && engineDto.GenerationId != generationDto.Id)
            return UnitResult.Failure(Error.Validation("car_snapshot.engine", "Engine does not belong to Generation"));


        if (generationDto is not null && patchedDto.Year is not null &&
            (patchedDto.Year < generationDto.YearFrom || patchedDto.Year > generationDto.YearTo))
        {
            return UnitResult.Failure(Error.Validation(
                "car_snapshot.year",
                "Provided year of production should be within boundaries of the generation production years"));
        }

        Guid? carId =
            brandChanged &&
            modelChanged &&
            generationChanged &&
            engineChanged &&
            bodyChanged &&
            transmissionChanged &&
            driveChanged
                ? null
                : currentCar?.CarId;

        if (brandDto is not null &&
            modelDto is not null &&
            generationDto is not null &&
            engineDto is not null &&
            bodyDto is not null &&
            transmissionDto is not null &&
            driveDto is not null)
        {
            var carIdResult = await autoCatalog.GetCarIdAsync(
                brandDto.Id,
                modelDto.Id,
                generationDto.Id,
                engineDto.Id,
                transmissionDto.Id,
                driveDto.Id,
                bodyDto.Id,
                ct);

            if (carIdResult.IsFailure) return carIdResult;

            carId = carIdResult.Value;
        }

        BrandSnapshot? brand = null;
        ModelSnapshot? model = null;
        GenerationSnapshot? generation = null;
        EngineSnapshot? engine = null;
        BodyTypeSnapshot? bodyType = null;
        TransmissionTypeSnapshot? transmissionType = null;
        AutoDriveTypeSnapshot? driveType = null;


        if (brandDto is not null)
        {
            var brandSnapshotResult = BrandSnapshot.Of(brandDto.Id, brandDto.Name);
            if (brandSnapshotResult.IsFailure) return brandSnapshotResult;
            brand = brandSnapshotResult.Value;
        }

        if (modelDto is not null)
        {
            var modelSnapshotResult = ModelSnapshot.Of(modelDto.Id, modelDto.Name, brandDto!.Id);
            if (modelSnapshotResult.IsFailure) return modelSnapshotResult;
            model = modelSnapshotResult.Value;
        }

        if (generationDto is not null)
        {
            var generationSnapshotResult = GenerationSnapshot.Of(
                generationDto.Id,
                generationDto.Name,
                modelDto!.Id,
                generationDto.YearFrom,
                generationDto.YearTo);

            if (generationSnapshotResult.IsFailure) return generationSnapshotResult;
            generation = generationSnapshotResult.Value;
        }

        if (engineDto is not null)
        {
            var fuelTypeSnapshotResult = FuelTypeSnapshot.Of(engineDto.FuelType.Id, engineDto.FuelType.Name);
            if (fuelTypeSnapshotResult.IsFailure) return fuelTypeSnapshotResult;

            var engineSnapshotResult = EngineSnapshot.Of(
                engineDto.Id,
                engineDto.Name,
                engineDto.HorsePower,
                fuelTypeSnapshotResult.Value,
                generationDto!.Id);

            if (engineSnapshotResult.IsFailure) return engineSnapshotResult;
            engine = engineSnapshotResult.Value;
        }

        if (driveDto is not null)
        {
            var driveTypeSnapshotResult = AutoDriveTypeSnapshot.Of(driveDto.Id, driveDto.Name);
            if (driveTypeSnapshotResult.IsFailure) return driveTypeSnapshotResult;
            driveType = driveTypeSnapshotResult.Value;
        }

        if (bodyDto is not null)
        {
            var bodyTypeSnapshotResult = BodyTypeSnapshot.Of(bodyDto.Id, bodyDto.Name);
            if (bodyTypeSnapshotResult.IsFailure) return bodyTypeSnapshotResult;
            bodyType = bodyTypeSnapshotResult.Value;
        }

        if (transmissionDto is not null)
        {
            var transmissionTypeSnapshotResult = TransmissionTypeSnapshot.Of(transmissionDto.Id, transmissionDto.Name);
            if (transmissionTypeSnapshotResult.IsFailure) return transmissionTypeSnapshotResult;
            transmissionType = transmissionTypeSnapshotResult.Value;
        }


        var newCarResult = CarSnapshot.Of(
            carId: carId,
            brand: brand,
            model: model,
            generation: generation,
            engine: engine,
            driveType: driveType,
            transmissionType: transmissionType,
            bodyType: bodyType,
            year: patchedDto.Year,
            vin: patchedDto.Vin,
            mileage: patchedDto.Mileage,
            color: patchedDto.Color,
            consumption: patchedDto.Consumption);

        if (newCarResult.IsFailure) return newCarResult;

        var adUpdateCarResult = ad.UpdateCar(newCarResult.Value);

        if (adUpdateCarResult.IsFailure) return adUpdateCarResult;

        await dbContext.SaveChangesAsync(ct);

        return UnitResult.Success<Error>();
    }
}