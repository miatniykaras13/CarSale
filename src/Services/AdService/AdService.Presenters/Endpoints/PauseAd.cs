using System.Security.Claims;
using AdService.Application.Commands.PauseAd;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AdService.Presenters.Endpoints;

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
                    return result.Error.ToResponse(context);

                return Results.Ok();
            })
            .RequireAuthorization()
            .WithName("PauseAd")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status200OK);
}