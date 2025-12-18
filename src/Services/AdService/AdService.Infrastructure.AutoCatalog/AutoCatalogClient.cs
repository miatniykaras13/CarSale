using AdService.Application.Abstractions.AutoCatalog;
using AdService.Contracts.AutoCatalog;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;

namespace AdService.Infrastructure.AutoCatalog;

public class AutoCatalogClient : IAutoCatalogClient
{
    public async Task<Result<BrandDto, Error>> GetBrandByIdAsync(int brandId, CancellationToken ct = default)
    {
        return Result.Success<BrandDto, Error>(new BrandDto(1, "Volkswagen", 1930, 2025));
    }

    public async Task<Result<ModelDto, Error>> GetModelByIdAsync(int modelId, CancellationToken ct = default)
    {
        return Result.Success<ModelDto, Error>(new ModelDto(2, "Jetta", 1));
    }

    public async Task<Result<GenerationDto, Error>> GetGenerationByIdAsync(int modelId, CancellationToken ct = default)
    {
        return Result.Success<GenerationDto, Error>(new GenerationDto(3, "IV", 2));
    }

    public async Task<Result<EngineDto, Error>> GetEngineByIdAsync(int engineId, CancellationToken ct = default)
    {
        return Result.Success<EngineDto, Error>(new EngineDto(4, 3, "1.9 TDI", 1, 1.9f, 90, 880));
    }

    public async Task<Result<TransmissionTypeDto, Error>> GetTransmissionTypeByIdAsync(
        int transmissionId,
        CancellationToken ct = default)
    {
        return Result.Success<TransmissionTypeDto, Error>(new TransmissionTypeDto(1, "Manual"));
    }

    public async Task<Result<AutoDriveTypeDto, Error>> GetAutoDriveTypeByIdAsync(
        int autoDriveId,
        CancellationToken ct = default)
    {
        return Result.Success<AutoDriveTypeDto, Error>(new AutoDriveTypeDto(1, "FWD"));
    }

    public async Task<Result<BodyTypeDto, Error>> GetBodyTypeByIdAsync(int bodyId, CancellationToken ct = default)
    {
        return Result.Success<BodyTypeDto, Error>(new BodyTypeDto(1, "Sedan"));
    }

    public async Task<Result<Guid, Error>> GetCarIdAsync(int brandId, int modelId, int generationId, int engineId,
        int transmissionId, int autoDriveTypeId,
        int bodyTypeId, CancellationToken ct = default)
    {
        return Guid.CreateVersion7();
    }

    public async Task<Result<FuelTypeDto, Error>> GetFuelTypeByIdAsync(
        int fuelTypeId,
        CancellationToken ct = default)
    {
        return Result.Success<FuelTypeDto, Error>(new FuelTypeDto(1, "Diesel"));

    }
}