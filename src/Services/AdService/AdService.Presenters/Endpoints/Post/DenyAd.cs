using System.Security.Claims;
using AdService.Application.Commands.DenyAd;
using AdService.Contracts.Ads.Default;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AdService.Presenters.Endpoints.Post;

public class DenyAd : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost("/ads/{adId:guid}/deny", async (
                HttpContext context,
                [FromRoute] Guid adId,
                [FromBody] ModerationResultDto moderationResultDto,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken ct) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                    return Results.Unauthorized();

                var command = new DenyAdCommand(adId, Guid.Parse(userId), moderationResultDto);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok();
            })
            .RequireAuthorization("ModeratorPolicy")
            .WithName("DenyAd")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status200OK)
            .WithTags("AdLifecycle")
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Deny ad",
                Description = "Rejects the advertisement during moderation. " +
                              "The request body must contain the deny reason and an optional message explaining the rejection. " +
                              "The ad transitions to the 'Denied' state and the owner is notified. " +
                              "Requires moderator privileges.",
            });
}