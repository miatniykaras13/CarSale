using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Cars.DeleteCar;

public class DeleteCarEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/cars/{id:guid}", async ([FromRoute] Guid id, ISender sender, CancellationToken ct = default) =>
            {
                var result = await sender.Send(new DeleteCarCommand(id), ct);

                if (result.IsFailure)
                    return result.ToResponse();

                return Results.Ok();
            })
            .WithName("DeleteCar")
            .Produces(StatusCodes.Status200OK)
            .ProducesGetProblems()
            .WithTags("Cars")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Delete car", Description = "Deletes car by id" });
    }
}