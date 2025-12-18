using AdService.Application.Abstractions.AutoCatalog;
using AdService.Application.Abstractions.Data;
using AdService.Domain.ValueObjects;

namespace AdService.Application.Commands.MergePatchAdsCar;

public class MergePatchAdsCarCommandHandler(
    IAppDbContext dbContext,
    IAutoCatalogClient autoCatalog)
    : ICommandHandler<MergePatchAdsCarCommand, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(MergePatchAdsCarCommand command, CancellationToken ct)
    {
        var ad = await dbContext.Ads.FindAsync([command.AdId], ct);

        if (ad is null)
            return UnitResult.Failure(Error.NotFound("ad", $"Ad with id {command.AdId} not found"));

        var carDto = command.CarDto;

        var currentCar = ad.Car;

        var mainSpecsProvided =
            carDto.BrandId is not null &&
            carDto.ModelId is not null &&
            carDto.GenerationId is not null &&
            carDto.EngineId is not null &&
            carDto.TransmissionTypeId is not null &&
            carDto.DriveTypeId is not null &&
            carDto.BodyTypeId is not null;

        var brandName = currentCar?.Brand;
        var modelName = currentCar?.Model;
        var generationName = currentCar?.Generation;
        var fuelName = currentCar?.FuelType;
        var transmissionName = currentCar?.TransmissionType;
        var driveName = currentCar?.DriveType;
        var bodyName = currentCar?.BodyType;
        var carId = currentCar?.CarId;

        if ((currentCar is null && mainSpecsProvided) ||
            (currentCar is not null && mainSpecsProvided))
        {
            var brandTask = autoCatalog.GetBrandByIdAsync(carDto.BrandId!.Value, ct);
            var modelTask = autoCatalog.GetModelByIdAsync(carDto.ModelId!.Value, ct);
            var generationTask = autoCatalog.GetGenerationByIdAsync(carDto.GenerationId!.Value, ct);
            var engineTask = autoCatalog.GetEngineByIdAsync(carDto.EngineId!.Value, ct);
            var transmissionTask = autoCatalog.GetTransmissionTypeByIdAsync(carDto.TransmissionTypeId!.Value, ct);
            var driveTask = autoCatalog.GetAutoDriveTypeByIdAsync(carDto.DriveTypeId!.Value, ct);
            var bodyTask = autoCatalog.GetBodyTypeByIdAsync(carDto.BodyTypeId!.Value, ct);

            await Task.WhenAll(brandTask, modelTask, generationTask, engineTask, transmissionTask, driveTask, bodyTask);

            var brandResult = await brandTask;
            var modelResult = await modelTask;
            var generationResult = await generationTask;
            var engineResult = await engineTask;
            var transmissionResult = await transmissionTask;
            var driveResult = await driveTask;
            var bodyResult = await bodyTask;

            if (brandResult.IsFailure) return brandResult;
            if (modelResult.IsFailure) return modelResult;
            if (generationResult.IsFailure) return generationResult;
            if (engineResult.IsFailure) return engineResult;
            if (transmissionResult.IsFailure) return transmissionResult;
            if (driveResult.IsFailure) return driveResult;
            if (bodyResult.IsFailure) return bodyResult;

            var brandDto = brandResult.Value;
            var modelDto = modelResult.Value;
            var generationDto = generationResult.Value;
            var engineDto = engineResult.Value;
            var transmissionDto = transmissionResult.Value;
            var driveDto = driveResult.Value;
            var bodyDto = bodyResult.Value;

            var fuelResult = await autoCatalog.GetFuelTypeByIdAsync(engineDto.FuelTypeId, ct);
            if (fuelResult.IsFailure) return fuelResult;

            var fuelDto = fuelResult.Value;

            if (modelDto.BrandId != brandDto.Id)
                return UnitResult.Failure(Error.Validation("car_snapshot", "Model does not belong to Brand"));

            if (generationDto.ModelId != modelDto.Id)
                return UnitResult.Failure(Error.Validation("car_snapshot", "Generation does not belong to Model"));

            if (engineDto.GenerationId != generationDto.Id)
                return UnitResult.Failure(Error.Validation("car_snapshot", "Engine does not belong to Generation"));

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
            brandName = brandDto.Name;
            modelName = modelDto.Name;
            generationName = generationDto.Name;
            fuelName = fuelDto.Name;
            transmissionName = transmissionDto.Name;
            driveName = driveDto.Name;
            bodyName = bodyDto.Name;
        }
        else if (currentCar is null && !mainSpecsProvided)
        {
            return UnitResult.Failure(
                Error.Validation(
                    "car_snapshot",
                    "BrandId, modelId, generationId, engineId, transmissionTypeId, driveTypeId must be provided if car wasn't created before"));
        }

        var newCarResult = CarSnapshot.Of(
            carId: carId!.Value,
            brand: brandName!,
            model: modelName!,
            generation: generationName!,
            driveType: driveName!,
            transmissionType: transmissionName!,
            bodyType: bodyName!,
            fuelType: fuelName!,
            year: carDto.Year ?? currentCar!.Year,
            vin: carDto.Vin ?? currentCar!.Vin,
            mileage: carDto.Mileage ?? currentCar!.Mileage,
            horsePower: carDto.HorsePower ?? currentCar!.HorsePower,
            color: carDto.Color ?? currentCar!.Color,
            consumption: carDto.Consumption ?? currentCar!.Consumption);

        if (newCarResult.IsFailure) return newCarResult;

        var adUpdateCarResult = ad.UpdateCar(newCarResult.Value);

        if (adUpdateCarResult.IsFailure) return adUpdateCarResult;

        await dbContext.SaveChangesAsync(ct);

        return UnitResult.Success<Error>();
    }
}