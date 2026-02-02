using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Cars.UpdateImage;


public record UpdateImageResponse(Guid Id);

public class UpdateImageEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPut("/cars/{id:guid}/image", async (
                HttpContext context,
                [FromRoute] Guid id,
                IFormFile file,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = new UpdateImageCommand(
                    id,
                    file.OpenReadStream(),
                    file.FileName,
                    file.ContentType);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToResponse(context);

                UpdateImageResponse response = new(result.Value);
                return Results.Ok(response);
            })
            .DisableAntiforgery()
            .WithName("UpdateImage")
            .Produces<UpdateImageResponse>(StatusCodes.Status200OK)
            .ProducesPostProblems()
            .WithTags("Cars")
            .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Update car image", Description = "Returns image id" });
}