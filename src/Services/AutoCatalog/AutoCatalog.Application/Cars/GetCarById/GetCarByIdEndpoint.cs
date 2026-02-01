using AutoCatalog.Application.Cars.Dtos;
using AutoCatalog.Application.Cars.GetCars;
using AutoCatalog.Application.Dtos;
using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Cars.GetCarById;

public record GetCarByIdResponse(
    Guid Id,
    BrandDto Brand,
    ModelDto Model,
    GenerationDto Generation,
    EngineDto Engine,
    TransmissionTypeDto TransmissionType,
    AutoDriveTypeDto DriveType,
    string PhotoUrl,
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
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Get car by id", Description = "Returns car with given id" });
    }
}