using AutoCatalog.Application.Extensions;
using AutoCatalog.Application.Models.PatchModel;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Models.PatchModel;

public record PatchModelRequest(string? Name);
public record PatchModelResponse(int Id);

public class PatchModelEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/models/{id:int}", async (
                [FromRoute] int id,
                [FromBody] PatchModelRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = new PatchModelCommand(id, request.Name);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.ToResponse();

                return Results.NoContent();
            })
            .WithName("PatchModel")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesPatchProblems()
            .WithTags("Models")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Patch Model", Description = "Returns model id" });
    }
}