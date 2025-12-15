using AdService.Application.Abstractions.AutoCatalog;
using AdService.Application.Abstractions.Data;
using AdService.Contracts.Cars;
using AdService.Domain.ValueObjects;

namespace AdService.Application.Commands.MergePatchAdsCar;

/*public class MergePatchAdsCarCommandHandler(IAppDbContext dbContext, IAutoCatalogClient autoCatalog)
    : ICommandHandler<MergePatchAdsCarCommand, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(UpdateCarSnapshotCommand request, CancellationToken ct)
    {
        var ad = await dbContext.Ads
            .Include(a => a.CarSnapshot) // если snapshot хранится в отдельной таблице, включи нужные навигации
            .FirstOrDefaultAsync(a => a.Id == request.AdId, ct);

        if (ad is null)
            return UnitResult.Failure<Error>(Error.NotFound("ad.not_found", "Ad not found"));

        // Статусная проверка: можно ли менять машину
        if (!ad.CanChangeCar()) // реализуй метод в домене
            return UnitResult.Failure<Error>(Error.Conflict("ad.cannot_change_car", "Cannot change car in current ad status"));

        var dto = request.Dto;
        var current = ad.CarSnapshot; // может быть null если ещё не установлен

        // Начинаем с текущих значений (если null — используем null)
        Guid? carId = dto.CarId != Guid.Empty ? dto.CarId : current?.CarId;
        int? brandId = dto.BrandId ?? /* если не прислано #1# GetBrandIdFromCurrent(current);
        int? modelId = dto.ModelId ?? GetModelIdFromCurrent(current);
        int? generationId = dto.GenerationId ?? GetGenerationIdFromCurrent(current);
        int? engineId = dto.EngineId ?? GetEngineIdFromCurrent(current);

        // Логика очистки зависимых полей при смене родителя
        // Если пришёл BrandId и он отличается от текущего -> сбрасываем model/generation/engine
        if (dto.BrandId is not null && dto.BrandId != GetBrandIdFromCurrent(current))
        {
            modelId = null;
            generationId = null;
            engineId = null;
        }

        // Если пришёл ModelId и он отличается -> сбрасываем generation/engine
        if (dto.ModelId is not null && dto.ModelId != GetModelIdFromCurrent(current))
        {
            generationId = null;
            engineId = null;
        }

        // Если пришёл GenerationId и он отличается -> сбрасываем engine
        if (dto.GenerationId is not null && dto.GenerationId != GetGenerationIdFromCurrent(current))
        {
            engineId = null;
        }

        // Проверки совместимости через каталог (если соответствующие id заданы)
        if (modelId is not null && brandId is not null)
        {
            var ok = await autoCatalog.IsModelOfBrandAsync(modelId.Value, brandId.Value, ct);
            if (!ok)
                return UnitResult.Failure<Error>(Error.Domain("car.model_brand.mismatch", "Model is not compatible with Brand"));
        }

        if (generationId is not null && modelId is not null)
        {
            var ok = await autoCatalog.IsGenerationOfModelAsync(generationId.Value, modelId.Value, ct);
            if (!ok)
                return UnitResult.Failure<Error>(Error.Domain("car.generation_model.mismatch", "Generation is not compatible with Model"));
        }

        if (engineId is not null && generationId is not null)
        {
            var ok = await autoCatalog.IsEngineOfGenerationAsync(engineId.Value, generationId.Value, ct);
            if (!ok)
                return UnitResult.Failure<Error>(Error.Domain("car.engine_generation.mismatch", "Engine is not compatible with Generation"));
        }

        
        var brandName = brandId is not null ? await autoCatalog.GetModelByIdAsync(brandId.Value, ct) : current?.Brand;
        var modelName = modelId is not null ? await autoCatalog.GetModelByIdAsync(modelId.Value, ct) : current?.Model;
        var generationName = generationId is not null ? await autoCatalog.GetModelByIdAsync(generationId.Value, ct) : current?.Generation;
        var engineName = engineId is not null ? await autoCatalog.GetModelByIdAsync(engineId.Value, ct) : null;

        // Остальные поля: если dto поле не null — берем его, иначе — текущее значение
        int? year = dto.Year ?? current?.Year;
        string? vin = dto.Vin ?? current?.Vin;
        int? mileage = dto.Mileage ?? current?.Mileage;
        decimal? consumption = dto.Consumption ?? current?.Consumption;
        string? color = dto.Color ?? current?.Color;
        int? horsePower = dto.HorsePower ?? current?.HorsePower;
        string? driveType = dto.DriveTypeId is not null ? await ResolveDriveType(dto.DriveTypeId.Value) : current?.DriveType;
        string? transmissionType = dto.TransmissionTypeId is not null ? await ResolveTransmission(dto.TransmissionTypeId.Value) : current?.TransmissionType;
        string? fuelType = dto.FuelTypeId is not null ? await ResolveFuelType(dto.FuelTypeId.Value) : current?.FuelType;

        // Создаём новый snapshot через фабрику и валидируем
        var carSnapshotResult = CarSnapshot.Of(
            carId,
            brandName,
            modelName,
            year,
            generationName,
            vin,
            mileage,
            color,
            horsePower,
            consumption,
            driveType,
            transmissionType,
            fuelType);

        if (carSnapshotResult.IsFailure)
            return UnitResult.Failure<Error>(carSnapshotResult.Error);

        var newSnapshot = carSnapshotResult.Value;

        // Применяем в домен
        var updateResult = ad.UpdateCarSnapshot(newSnapshot);
        if (updateResult.IsFailure)
            return UnitResult.Failure<Error>(updateResult.Error);

        await dbContext.SaveChangesAsync(ct);

        return Result.Success(_mapper.Map<AdDto>(ad));
    }
}*/