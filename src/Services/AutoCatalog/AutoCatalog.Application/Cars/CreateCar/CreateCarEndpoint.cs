using AutoCatalog.Application.Cars.Dtos;
using AutoCatalog.Application.Dtos;
using BuildingBlocks.Extensions;
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
    int TransmissionTypeId,
    int DriveTypeId,
    int BodyTypeId,
    float Consumption,
    float Acceleration,
    int FuelTankCapacity,
    DimensionsDto DimensionsDto);

public record CreateCarResponse(Guid Id);

public class CreateCarEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost("/cars", async (
                HttpContext context,
                CreateCarRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = request.Adapt<CreateCarCommand>();

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                CreateCarResponse response = new(result.Value);
                return Results.Created($"/cars/{response.Id}", response);
            })
            .RequireAuthorization("AdminPolicy")
            .WithName("CreateCar")
            .Produces<CreateCarResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags("Cars")
            .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Create car", Description = "Returns car id" });
}