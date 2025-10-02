using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Generations.PatchGeneration;

public record PatchGenerationRequest(string? Name);
public record PatchGenerationResponse(int Id);

public class PatchGenerationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/generations/{id:int}", async (
                [FromRoute] int id,
                [FromBody] PatchGenerationRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = new PatchGenerationCommand(id, request.Name);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.ToResponse();

                return Results.NoContent();
            })
            .WithName("PatchGeneration")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags("Generations")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Patch Generation", Description = "Returns generation id" });
    }
}