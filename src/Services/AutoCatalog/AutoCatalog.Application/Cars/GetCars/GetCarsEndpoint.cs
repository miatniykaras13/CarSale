using AutoCatalog.Application.Extensions;
using AutoCatalog.Domain.Enums;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Cars.GetCars;

public record GetCarResponse(
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
    DimensionsDto Dimensions
);

public class GetCarsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/cars", async (
                [AsParameters] CarFilter filter,
                [AsParameters] SortParameters sortParameters,
                [AsParameters] PageParameters pageParameters,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var result = await sender.Send(new GetCarsQuery(filter, sortParameters, pageParameters), ct);

                if (result.IsFailure)
                    return result.ToResponse();
                var response = result.Value.Adapt<List<GetCarResponse>>();
                return Results.Ok(response);
            })
            .RequireAuthorization()
            .WithName("GetCars")
            .Produces<List<GetCarResponse>>(StatusCodes.Status200OK)
            .ProducesGetProblems()
            .WithTags("Cars")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Get cars", Description = "Returns the list of cars" });
    }
}