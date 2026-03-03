using System.Security.Claims;
using AdService.Application.Commands.RestoreAd;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AdService.Presenters.Endpoints.Post;

public class RestoreAd : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost("/ads/{adId:guid}/restore", async (
                HttpContext context,
                [FromRoute] Guid adId,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken ct) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                    return Results.Unauthorized();

                var command = new RestoreAdCommand(adId, Guid.Parse(userId));

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok();
            })
            .RequireAuthorization()
            .WithName("RestoreAd")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags("AdLifecycle")
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Restore ad",
                Description = "Restores a previously deleted advertisement back to the draft state if ad was published or submitted. " +
                              "Only ads in the 'Deleted' state can be restored. " +
                              "Only the ad owner can restore their ad. " +
                              "Returns 404 if the ad does not exist.",
            });
}