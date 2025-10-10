using AutoCatalog.Domain.Enums;
using BuildingBlocks.Extensions;
using Carter;
using CSharpFunctionalExtensions;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Cars.CreateCar;

public record CreateCarRequest(
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
    DimensionsDto DimensionsDto);

public record CreateCarResponse(Guid Id);

public class CreateCarEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost("/cars", async (CreateCarRequest request, ISender sender, CancellationToken ct = default) =>
            {
                var command = request.Adapt<CreateCarCommand>();

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.ToResponse();

                CreateCarResponse response = new(result.Value);
                return Results.Created($"/cars/{response.Id}", response);
            })
            .WithName("CreateCar")
            .Produces<CreateCarResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags("Cars")
            .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Create car", Description = "Returns car id" });
}