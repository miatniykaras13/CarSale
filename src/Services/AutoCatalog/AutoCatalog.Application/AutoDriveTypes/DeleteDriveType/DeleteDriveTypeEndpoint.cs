using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.AutoDriveTypes.DeleteDriveType;

public class DeleteDriveTypesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/drive-types/{id:int}", async (
                HttpContext context,
                [FromRoute] int id,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var result = await sender.Send(new DeleteDriveTypeCommand(id), ct);

                if (result.IsFailure)
                    return result.Error.ToResponse(context);

                return Results.NoContent();
            })
            .WithName("DeleteDriveType")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesDeleteProblems()
            .WithTags("DriveTypes")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Delete drive type", Description = "Deletes drive type by id" });
    }
}