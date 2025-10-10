using AutoCatalog.Application.Extensions;
using AutoCatalog.Domain.Enums;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Cars.GetCarsByModelId;

public record GetCarsByModelIdResponse(
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
    decimal Consumption,
    decimal Acceleration,
    int FuelTankCapacity,
    DimensionsDto Dimensions);

public class GetCarsByModelIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/models/{modelId:int}/cars", async (
                [AsParameters] CarFilter filter,
                [AsParameters] SortParameters sortParameters,
                [AsParameters] PageParameters pageParameters,
                [FromRoute] int modelId,
                ISender sender,
                CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetCarsByModelIdQuery(filter, sortParameters, pageParameters, modelId), ct);

            if (result.IsFailure)
                return result.ToResponse();

            var response = result.Value.Adapt<List<GetCarsByModelIdResponse>>();
            return Results.Ok(response);
        })
        .WithName("GetCarsByModelId")
        .Produces<GetCarsByModelIdResponse>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("Cars")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get cars by model id", Description = "Returns cars with given model id" });
    }
}