using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Models.DeleteModel;

public class DeleteModelsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/models/{id:int}", async ([FromRoute] int id, ISender sender, CancellationToken ct = default) =>
            {
                var result = await sender.Send(new DeleteModelCommand(id), ct);

                if (result.IsFailure)
                    return result.ToResponse();

                return Results.Ok();
            })
            .WithName("DeleteModels")
            .Produces(StatusCodes.Status200OK)
            .ProducesGetProblems()
            .WithTags("Models")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Delete models", Description = "Deletes model by id" });
    }
}