using AutoCatalog.Application.Cars.Dtos;
using AutoCatalog.Application.Dtos;
using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Cars.UpdateCar;

public record UpdateCarRequest(
    int BrandId,
    int ModelId,
    int GenerationId,
    int EngineId,
    int BodyTypeId,
    int TransmissionTypeId,
    int DriveTypeId,
    float Consumption,
    float Acceleration,
    int FuelTankCapacity,
    DimensionsDto DimensionsDto);

public record UpdateCarResponse(Guid Id);

public class UpdateCarEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPut("/car/{id:guid}", async (
                HttpContext context,
                [FromRoute] Guid id,
                [FromBody] UpdateCarRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = new UpdateCarCommand(
                    id,
                    request.BrandId,
                    request.ModelId,
                    request.GenerationId,
                    request.EngineId,
                    request.BodyTypeId,
                    request.TransmissionTypeId,
                    request.DriveTypeId,
                    request.Consumption,
                    request.Acceleration,
                    request.FuelTankCapacity,
                    request.DimensionsDto);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                UpdateCarResponse response = new(result.Value);
                return Results.Ok(response);
            })
            .RequireAuthorization("AdminPolicy")
            .WithName("UpdateCar")
            .Produces<UpdateCarResponse>(StatusCodes.Status200OK)
            .ProducesUpdateProblems()
            .WithTags("Cars")
            .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Update car", Description = "Returns car id" });
}