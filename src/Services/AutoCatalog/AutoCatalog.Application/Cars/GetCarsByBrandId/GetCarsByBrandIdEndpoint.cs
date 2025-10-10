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

namespace AutoCatalog.Application.Cars.GetCarsByBrandId;

public record GetCarsByBrandIdResponse(
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

public class GetCarsByBrandIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/brands/{brandId:int}/cars", async (
                [AsParameters] CarFilter filter,
                [AsParameters] SortParameters sortParameters,
                [AsParameters] PageParameters pageParameters,
                [FromRoute] int brandId,
                ISender sender,
                CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetCarsByBrandIdQuery(filter, sortParameters, pageParameters, brandId), ct);

            if (result.IsFailure)
                return result.ToResponse();

            var response = result.Value.Adapt<List<GetCarsByBrandIdResponse>>();
            return Results.Ok(response);
        })
        .WithName("GetCarsByBrandId")
        .Produces<GetCarsByBrandIdResponse>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("Cars")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get cars by brand id", Description = "Returns cars with given brand id" });
    }
}