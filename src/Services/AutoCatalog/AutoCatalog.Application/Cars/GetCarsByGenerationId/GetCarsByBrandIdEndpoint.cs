using AutoCatalog.Application.Extensions;
using AutoCatalog.Domain.Enums;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Cars.GetCarsByGenerationId;

public record GetCarsByGenerationIdResponse(
    Guid Id,
    int BrandId,
    int ModelId,
    int GenerationId,
    int EngineId,
    TransmissionType TransmissionType,
    AutoDriveType AutoDriveType,
    int YearFrom,
    int YearTo,
    Guid PhotoId,
    float Consumption,
    float Acceleration,
    int FuelTankCapacity,
    DimensionsDto Dimensions);

public class GetCarsByGenerationIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/generations/{generationId:int}/cars", async ([FromRoute] int generationId, ISender sender, CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetCarsByGenerationIdQuery(generationId), ct);

            if (result.IsFailure)
                return result.ToResponse();

            var response = result.Value.Adapt<List<GetCarsByGenerationIdResponse>>();
            return Results.Ok(response);
        })
        .WithName("GetCarsByGenerationId")
        .Produces<GetCarsByGenerationIdResponse>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("Cars")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get cars by generation id", Description = "Returns cars with given generation id" });
    }
}