using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.AutoDriveTypes.UpdateDriveType;

public record UpdateDriveTypeRequest(string Name);

public record UpdateDriveTypeResponse(int Id);

public class UpdateDriveTypeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/drive-types/{id:int}", async (
                HttpContext context,
                [FromRoute] int id,
                [FromBody] UpdateDriveTypeRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = new UpdateDriveTypeCommand(id, request.Name);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToResponse(context);

                return Results.Ok(new UpdateDriveTypeResponse(result.Value));
            })
            .WithName("UpdateDriveType")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesUpdateProblems()
            .WithTags("DriveTypes")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Update drive type", Description = "Returns drive type id" });
    }
}