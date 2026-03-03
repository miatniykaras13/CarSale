using System.Security.Claims;
using AdService.Application.Commands.PauseAd;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AdService.Presenters.Endpoints.Post;

public class PauseAd : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost("/ads/{adId:guid}/pause", async (
                HttpContext context,
                [FromRoute] Guid adId,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken ct) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                    return Results.Unauthorized();

                var command = new PauseAdCommand(adId, Guid.Parse(userId));

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok();
            })
            .RequireAuthorization()
            .WithName("PauseAd")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status200OK)
            .WithTags("AdLifecycle")
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Pause ad",
                Description = "Temporarily pauses a published advertisement. " +
                              "Paused ads are hidden from search results but can be resumed by the owner. " +
                              "Only the ad owner can pause their ad.",
            });
}