using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.TransmissionTypes.DeleteTransmissionType;

public class DeleteTransmissionTypesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/transmission-types/{id:int}", async (
                HttpContext context,
                [FromRoute] int id,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var result = await sender.Send(new DeleteTransmissionTypeCommand(id), ct);

                if (result.IsFailure)
                    return result.Error.ToResponse(context);

                return Results.NoContent();
            })
            .WithName("DeleteTransmissionType")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesDeleteProblems()
            .WithTags("TransmissionTypes")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Delete transmission type", Description = "Deletes transmission type by id" });
    }
}