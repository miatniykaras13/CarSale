using System.Net.Http.Json;
using System.Text.Json;
using AdService.Application.Abstractions.AutoCatalog;
using AdService.Application.Builders;
using AdService.Contracts.AutoCatalog.Cars;
using AdService.Contracts.AutoCatalog.FuelTypes;
using BuildingBlocks.Errors;
using BuildingBlocks.Extensions;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;
using AutoDriveTypeDto = AdService.Contracts.AutoCatalog.AutoDriveTypes.AutoDriveTypeDto;
using BodyTypeDto = AdService.Contracts.AutoCatalog.BodyTypes.BodyTypeDto;
using BrandDto = AdService.Contracts.AutoCatalog.Brands.BrandDto;
using EngineDto = AdService.Contracts.AutoCatalog.Engines.EngineDto;
using GenerationDto = AdService.Contracts.AutoCatalog.Generations.GenerationDto;
using ModelDto = AdService.Contracts.AutoCatalog.Models.ModelDto;
using TransmissionTypeDto = AdService.Contracts.AutoCatalog.TransmissionTypes.TransmissionTypeDto;

namespace AdService.Infrastructure.AutoCatalog;

public class AutoCatalogClient(
    HttpClient httpClient,
    HybridCache cache) : IAutoCatalogClient
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = JsonSerializerOptions.Web;

    public async Task<Result<BrandDto, Error>> GetBrandByIdAsync(int brandId, CancellationToken ct = default)
    {
        var cacheKey = CacheKeyBuilder.Build(nameof(BrandDto), brandId.ToString());

        var brandDto = await GetCachedValueAsync<BrandDto>(cacheKey, ct);
        if (brandDto is not null)
            return brandDto;

        var response = await httpClient.GetAsync($"brands/{brandId}", ct);
        return await HandleResponseAsync<BrandDto>(response, cacheKey, ct);
    }

    public async Task<Result<ModelDto, Error>> GetModelByIdAsync(int modelId, CancellationToken ct = default)
    {
        var cacheKey = CacheKeyBuilder.Build(nameof(ModelDto), modelId.ToString());
        var modelDto = await GetCachedValueAsync<ModelDto?>(cacheKey, ct);
        if (modelDto is not null)
            return modelDto;

        var response = await httpClient.GetAsync($"models/{modelId}", ct);
        return await HandleResponseAsync<ModelDto>(response, cacheKey, ct);
    }

    public async Task<Result<GenerationDto, Error>> GetGenerationByIdAsync(
        int generationId,
        CancellationToken ct = default)
    {
        var cacheKey = CacheKeyBuilder.Build(nameof(GenerationDto), generationId.ToString());
        var generationDto = await GetCachedValueAsync<GenerationDto?>(cacheKey, ct);
        if (generationDto is not null)
            return generationDto;

        var response = await httpClient.GetAsync($"generations/{generationId}", ct);
        return await HandleResponseAsync<GenerationDto>(response, cacheKey, ct);
    }

    public async Task<Result<EngineDto, Error>> GetEngineByIdAsync(int engineId, CancellationToken ct = default)
    {
        var cacheKey = CacheKeyBuilder.Build(nameof(EngineDto), engineId.ToString());
        var engineDto = await GetCachedValueAsync<EngineDto?>(cacheKey, ct);
        if (engineDto is not null)
            return engineDto;
        var response = await httpClient.GetAsync($"engines/{engineId}", ct);
        return await HandleResponseAsync<EngineDto>(response, cacheKey, ct);
    }

    public async Task<Result<TransmissionTypeDto, Error>> GetTransmissionTypeByIdAsync(
        int transmissionId,
        CancellationToken ct = default)
    {
        var cacheKey = CacheKeyBuilder.Build(nameof(TransmissionTypeDto), transmissionId.ToString());
        var transmissionTypeDto = await GetCachedValueAsync<TransmissionTypeDto?>(cacheKey, ct);
        if (transmissionTypeDto is not null)
            return transmissionTypeDto;

        var response = await httpClient.GetAsync($"transmission-types/{transmissionId}", ct);
        return await HandleResponseAsync<TransmissionTypeDto>(response, cacheKey, ct);
    }

    public async Task<Result<AutoDriveTypeDto, Error>> GetAutoDriveTypeByIdAsync(
        int driveTypeId,
        CancellationToken ct = default)
    {
        var cacheKey = CacheKeyBuilder.Build(nameof(AutoDriveTypeDto), driveTypeId.ToString());
        var driveTypeDto = await GetCachedValueAsync<AutoDriveTypeDto?>(cacheKey, ct);
        if (driveTypeDto is not null)
            return driveTypeDto;

        var response = await httpClient.GetAsync($"drive-types/{driveTypeId}", ct);
        return await HandleResponseAsync<AutoDriveTypeDto>(response, cacheKey, ct);
    }

    public async Task<Result<BodyTypeDto, Error>> GetBodyTypeByIdAsync(int bodyTypeId, CancellationToken ct = default)
    {
        var cacheKey = CacheKeyBuilder.Build(nameof(BodyTypeDto), bodyTypeId.ToString());
        var bodyTypeDto = await GetCachedValueAsync<BodyTypeDto?>(cacheKey, ct);
        if (bodyTypeDto is not null)
            return bodyTypeDto;

        var response = await httpClient.GetAsync($"body-types/{bodyTypeId}", ct);
        return await HandleResponseAsync<BodyTypeDto>(response, cacheKey, ct);
    }

    public async Task<Result<Guid, Error>> GetCarIdAsync(
        int brandId,
        int modelId,
        int generationId,
        int engineId,
        int transmissionTypeId,
        int driveTypeId,
        int bodyTypeId,
        CancellationToken ct = default)
    {
        var cacheKey = CacheKeyBuilder.Build(nameof(CarDto), modelId.ToString());
        var carDtos = await GetCachedValueAsync<List<CarDto>>(cacheKey, ct);
        if (carDtos is not null)
            return carDtos[0].Id;

        var query = $"cars?" +
                    $"BrandId={brandId}&" +
                    $"ModelId={modelId}&" +
                    $"GenerationId={generationId}&" +
                    $"EngineId={engineId}&" +
                    $"TransmissionTypeId={transmissionTypeId}&" +
                    $"BodyTypeId={bodyTypeId}&" +
                    $"DriveTypeId={driveTypeId}";

        var response = await httpClient.GetAsync(query, ct);

        var carsResult = await HandleResponseAsync<List<CarDto>>(response, cacheKey, ct);

        if (carsResult.IsFailure) return Result.Failure<Guid, Error>(carsResult.Error);

        var cars = carsResult.Value;

        if (cars.Count == 0)
            return Result.Failure<Guid, Error>(Error.NotFound("car", "Car with specified id's not found"));

        return cars[0].Id;
    }

    public async Task<Result<FuelTypeDto, Error>> GetFuelTypeByIdAsync(
        int fuelTypeId,
        CancellationToken ct = default)
    {
        var cacheKey = CacheKeyBuilder.Build(nameof(FuelTypeDto), fuelTypeId.ToString());
        var fuelTypeDto = await GetCachedValueAsync<FuelTypeDto?>(cacheKey, ct);
        if (fuelTypeDto is not null)
            return fuelTypeDto;

        var response = await httpClient.GetAsync($"fuel-types/{fuelTypeId}", ct);
        return await HandleResponseAsync<FuelTypeDto>(response, cacheKey, ct);
    }

    private async Task<Result<T, Error>> HandleResponseAsync<T>(
        HttpResponseMessage response,
        string cacheKey,
        CancellationToken ct = default)
    {
        if (!response.IsSuccessStatusCode)
        {
            var problemDetails =
                await response.Content.ReadFromJsonAsync<ProblemDetails>(_jsonSerializerOptions, ct) ??
                throw new InvalidDataException("Serialized problem details are invalid");
            return Result.Failure<T, Error>(problemDetails.ToError());
        }

        var value = await response.Content.ReadFromJsonAsync<T>(_jsonSerializerOptions, ct);
        if (value is null)
        {
            return Result.Failure<T, Error>(Error.Internal(
                "deserialization_failed",
                $"Failed to deserialize response to type {typeof(T)}"));
        }

        await cache.SetAsync(cacheKey, value, cancellationToken: ct);

        return value;
    }

    private async Task<T?> GetCachedValueAsync<T>(string cacheKey, CancellationToken ct = default) =>
        await cache.GetOrCreateAsync<T?>(
            cacheKey,
            factory: null!,
            options: new HybridCacheEntryOptions() { Flags = HybridCacheEntryFlags.DisableUnderlyingData },
            cancellationToken: ct);
}