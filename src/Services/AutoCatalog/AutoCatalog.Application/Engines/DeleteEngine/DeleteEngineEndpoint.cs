using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Engines.DeleteEngine;

public class DeleteEnginesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/engines/{id:int}", async ([FromRoute] int id, ISender sender, CancellationToken ct = default) =>
            {
                var result = await sender.Send(new DeleteEngineCommand(id), ct);

                if (result.IsFailure)
                    return result.ToResponse();

                return Results.NoContent();
            })
            .WithName("DeleteEngine")
            .Produces(StatusCodes.Status200OK)
            .ProducesDeleteProblems()
            .WithTags("Engines")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Delete engine", Description = "Deletes engine by id" });
    }
}