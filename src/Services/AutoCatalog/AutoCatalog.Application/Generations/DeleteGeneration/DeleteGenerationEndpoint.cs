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
        app.MapDelete("/generations/{id:int}", async ([FromRoute] int id, ISender sender, CancellationToken ct = default) =>
            {
                var result = await sender.Send(new DeleteGenerationCommand(id), ct);

                if (result.IsFailure)
                    return result.ToResponse();

                return Results.Ok();
            })
            .WithName("DeleteGenerations")
            .Produces(StatusCodes.Status200OK)
            .ProducesGetProblems()
            .WithTags("Generations")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Delete generations", Description = "Deletes generation by id" });
    }
}