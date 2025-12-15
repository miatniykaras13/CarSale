using AdService.Contracts.AutoCatalog;

namespace AdService.Application.Abstractions.AutoCatalog;

public interface IAutoCatalogClient
{
    public Task<Result<BrandDto, Error>> GetBrandByIdAsync(int brandId);

    public Task<Result<ModelDto, Error>> GetModelByIdAsync(int modelId);

    public Task<Result<GenerationDto, Error>> GetGenerationByIdAsync(int modelId);

    public Task<Result<EngineDto, Error>> GetEngineByIdAsync(int engineId);

    public Task<bool> IsModelOfBrandAsync(int modelId);

    public Task<bool> IsGenerationOfModelAsync(int generationId);

    public Task<bool> IsEngineOfGenerationAsync(int engineId);

    public Task<Result<Guid, Error>> GetCarIdAsync(
        int brandId,
        int modelId,
        int generationId,
        int engineId,
        int transmissionId,
        int autoDriveTypeId);

    public Task<Result<CarDto, Error>> GetCarByIdAsync(Guid carId);
}