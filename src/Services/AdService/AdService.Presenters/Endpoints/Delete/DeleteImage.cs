using System.Security.Claims;
using AdService.Application.Commands.DeleteImage;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AdService.Presenters.Endpoints.Delete;

public class DeleteImage : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapDelete("/ads/{adId:guid}/images/{imageId:guid}", async (
                HttpContext context,
                [FromRoute] Guid adId,
                [FromRoute] Guid imageId,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken ct) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                    return Results.Unauthorized();

                var command = new DeleteImageCommand(adId, imageId, Guid.Parse(userId));

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok();
            })
            .RequireAuthorization()
            .WithName("DeleteImage")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status200OK)
            .WithTags("AdImages")
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Delete ad image",
                Description = "Removes an image from the specified advertisement. " +
                              "Only the ad owner can delete images. " +
                              "Requires both the ad identifier and the image identifier.",
            });
}