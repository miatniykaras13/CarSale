using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Engines.UpdateEngine;

public record UpdateEngineRequest(
    int GenerationId,
    string Name,
    int FuelTypeId,
    float Volume,
    int HorsePower,
    int TorqueNm);

public record UpdateEngineResponse(int Id);

public class UpdateEngineEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/engines/{id:int}", async (
                [FromRoute] int id,
                [FromBody] UpdateEngineRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = new UpdateEngineCommand(
                    id,
                    request.GenerationId,
                    request.Name,
                    request.FuelTypeId,
                    request.Volume,
                    request.HorsePower,
                    request.TorqueNm);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.ToResponse();

                UpdateEngineResponse response = new(result.Value);

                return Results.Ok(response);
            })
            .WithName("UpdateEngine")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesUpdateProblems()
            .WithTags("Engines")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Update Engine", Description = "Returns engine id" });
    }
}