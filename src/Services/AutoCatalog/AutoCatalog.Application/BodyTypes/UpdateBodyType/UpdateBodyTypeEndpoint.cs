using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.BodyTypes.UpdateBodyType;

public record UpdateBodyTypeRequest(string Name);

public record UpdateBodyTypeResponse(int Id);

public class UpdateBodyTypeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/body-types/{id:int}", async (
                [FromRoute] int id,
                [FromBody] UpdateBodyTypeRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = new UpdateBodyTypeCommand(id, request.Name);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToResponse();

                return Results.Ok(new UpdateBodyTypeResponse(result.Value));
            })
            .WithName("UpdateBodyType")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesUpdateProblems()
            .WithTags("BodyTypes")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Update body type", Description = "Returns body type id" });
    }
}