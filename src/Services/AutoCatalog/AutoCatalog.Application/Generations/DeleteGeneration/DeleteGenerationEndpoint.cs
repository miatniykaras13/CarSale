using AutoCatalog.Application.Extensions;
using AutoCatalog.Application.Generations.DeleteGeneration;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Generations.DeleteGeneration;

public class DeleteGenerationsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/generations/{id:int}", async (
                HttpContext context,
                [FromRoute] int id,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var result = await sender.Send(new DeleteGenerationCommand(id), ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.NoContent();
            })
            .WithName("DeleteGeneration")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesDeleteProblems()
            .WithTags("Generations")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Delete generation", Description = "Deletes generation by id" });
    }
}