using AutoCatalog.Domain.Enums;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Cars.PatchCar;

public record PatchCarRequest(
    TransmissionType? TransmissionType,
    AutoDriveType? AutoDriveType,
    int? YearFrom,
    int? YearTo,
    Guid? PhotoId,
    float? Consumption,
    float? Acceleration,
    int? FuelTankCapacity,
    DimensionsDto? DimensionsDto);

public record PatchCarResponse(Guid Id);

public class PatchCarEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPatch("/car/{id:guid}", async ([FromRoute] Guid id, [FromBody] PatchCarRequest request, ISender sender, CancellationToken ct = default) =>
            {
                var command = new PatchCarCommand(
                    id,
                    request.TransmissionType,
                    request.AutoDriveType,
                    request.YearFrom,
                    request.YearTo,
                    request.PhotoId,
                    request.Consumption,
                    request.Acceleration,
                    request.FuelTankCapacity,
                    request.DimensionsDto);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.ToResponse();

                PatchCarResponse response = new(result.Value);
                return Results.Ok();
            })
            .WithName("PatchCar")
            .Produces<PatchCarResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags("Cars")
            .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Patch car", Description = "Returns car id" });
}