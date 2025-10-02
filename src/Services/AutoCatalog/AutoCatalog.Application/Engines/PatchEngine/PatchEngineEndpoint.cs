using AutoCatalog.Application.Engines.PatchEngine;
using AutoCatalog.Domain.Enums;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Engines.PatchEngine;

public record PatchEngineRequest(string? Name);

public record PatchEngineResponse(int Id);

public class PatchEngineEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/engines/{id:int}", async (
                [FromRoute] int id,
                [FromBody] PatchEngineRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = new PatchEngineCommand(id, request.Name);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.ToResponse();

                PatchEngineResponse response = new(result.Value);

                return Results.Ok(response);
            })
            .WithName("PatchEngine")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags("Engines")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Patch Engine", Description = "Returns engine id" });
    }
}