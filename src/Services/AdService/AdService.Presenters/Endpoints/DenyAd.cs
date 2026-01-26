using System.Security.Claims;
using AdService.Application.Commands.DenyAd;
using AdService.Contracts.Ads.Default;
using AdService.Domain.ValueObjects;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AdService.Presenters.Endpoints;

public class DenyAd : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost("/ads/{adId:guid}/deny", async (
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
                    return result.Error.ToResponse();

                return Results.Ok();
            })
            .RequireAuthorization()
            .WithName("DenyAd")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status200OK);
}