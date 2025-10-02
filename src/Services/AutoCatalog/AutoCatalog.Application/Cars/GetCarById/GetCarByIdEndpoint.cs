using AutoCatalog.Application.Cars.GetCars;
using AutoCatalog.Application.Extensions;
using AutoCatalog.Domain.Enums;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Cars.GetCarById;

public record GetCarByIdResponse(
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

public class GetCarByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/cars/{id:guid}", async ([FromRoute] Guid id, ISender sender, CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetCarByIdQuery(id), ct);

            if (result.IsFailure)
                return result.ToResponse();

            var response = result.Value.Adapt<GetCarResponse>();
            return Results.Ok(response);
        })
        .WithName("GetCarById")
        .Produces<GetCarByIdResponse>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("Cars")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get car by id", Description = "Returns car with given id" });
    }
}