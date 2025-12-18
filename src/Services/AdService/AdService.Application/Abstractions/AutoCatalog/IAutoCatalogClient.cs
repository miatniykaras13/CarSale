using AdService.Contracts.AutoCatalog;
using AdService.Domain.Enums;

namespace AdService.Application.Abstractions.AutoCatalog;

public interface IAutoCatalogClient
{
    Task<Result<BrandDto, Error>> GetBrandByIdAsync(
        int brandId,
        CancellationToken ct = default);

    Task<Result<ModelDto, Error>> GetModelByIdAsync(
        int modelId,
        CancellationToken ct = default);

    Task<Result<GenerationDto, Error>> GetGenerationByIdAsync(
        int modelId,
        CancellationToken ct = default);

    Task<Result<EngineDto, Error>> GetEngineByIdAsync(
        int engineId,
        CancellationToken ct = default);

    Task<Result<TransmissionTypeDto, Error>> GetTransmissionTypeByIdAsync(
        int transmissionId,
        CancellationToken ct = default);

    Task<Result<AutoDriveTypeDto, Error>> GetAutoDriveTypeByIdAsync(
        int autoDriveId,
        CancellationToken ct = default);

    Task<Result<BodyTypeDto, Error>> GetBodyTypeByIdAsync(
        int bodyId,
        CancellationToken ct = default);


    Task<Result<Guid, Error>> GetCarIdAsync(
        int brandId,
        int modelId,
        int generationId,
        int engineId,
        int transmissionId,
        int autoDriveTypeId,
        int bodyTypeId,
        CancellationToken ct = default);

    Task<Result<FuelTypeDto, Error>> GetFuelTypeByIdAsync(int fuelTypeId, CancellationToken ct = default);
}