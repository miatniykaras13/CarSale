using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Generations.UpdateGeneration;

public record UpdateGenerationRequest(string Name, int YearFrom, int? YearTo);
public record UpdateGenerationResponse(int Id);

public class UpdateGenerationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/generations/{id:int}", async (
                HttpContext context,
                [FromRoute] int id,
                [FromBody] UpdateGenerationRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = new UpdateGenerationCommand(id, request.Name, request.YearFrom, request.YearTo);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.NoContent();
            })
            .WithName("UpdateGeneration")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesUpdateProblems()
            .WithTags("Generations")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Update Generation", Description = "Returns generation id" });
    }
}