using System.Net.Http.Json;
using System.Text.Json;
using AdService.Application.Abstractions.AutoCatalog;
using AdService.Contracts.AutoCatalog;
using AdService.Infrastructure.AutoCatalog.Options;
using BuildingBlocks.Errors;
using BuildingBlocks.Extensions;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AdService.Infrastructure.AutoCatalog;

public class AutoCatalogClient(IOptions<AutoCatalogOptions> options, HttpClient httpClient) : IAutoCatalogClient
{
    private readonly Uri _baseAddress = new(
        options.Value.Endpoint ??
        throw new InvalidDataException("Auto catalog endpoint is missing"));

    private readonly JsonSerializerOptions _jsonSerializerOptions = JsonSerializerOptions.Web;

    public async Task<Result<BrandDto, Error>> GetBrandByIdAsync(int brandId, CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync($"{_baseAddress}brands/{brandId}", ct);
        if (!response.IsSuccessStatusCode)
        {
            var problemDetails =
                await response.Content.ReadFromJsonAsync<ProblemDetails>(_jsonSerializerOptions, ct) ??
                throw new InvalidDataException("Serialized problem details are invalid");
            return Result.Failure<BrandDto, Error>(problemDetails.ToError());
        }

        var brandJson = await response.Content.ReadAsStringAsync(ct);

        var brandDto = JsonSerializer.Deserialize<BrandDto>(brandJson, _jsonSerializerOptions) ??
                       throw new JsonException("Failed to deserialize brand dto");

        return brandDto;
    }

    public async Task<Result<ModelDto, Error>> GetModelByIdAsync(int modelId, CancellationToken ct = default)
    {
        return Result.Success<ModelDto, Error>(new ModelDto(2, "Jetta", 1));
    }

    public async Task<Result<GenerationDto, Error>> GetGenerationByIdAsync(int modelId, CancellationToken ct = default)
    {
        return Result.Success<GenerationDto, Error>(new GenerationDto(3, "IV", 2, 2000, 2005));
    }

    public async Task<Result<EngineDto, Error>> GetEngineByIdAsync(int engineId, CancellationToken ct = default)
    {
        return Result.Success<EngineDto, Error>(new EngineDto(4, 3, "1.9 TDI", new FuelTypeDto(1, "Diesel"), 1.9f, 90,
            880));
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