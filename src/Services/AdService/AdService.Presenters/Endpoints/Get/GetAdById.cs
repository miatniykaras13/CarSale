using System.Security.Claims;
using AdService.Application.Queries.GetAdById;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AdService.Presenters.Endpoints.Get;

public class GetAdById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapGet("/ads/{adId:guid}", async (
                HttpContext context,
                [FromRoute] Guid adId,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                var command = new GetAdByIdQuery(adId, userId is null ? null : Guid.Parse(userId));
                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok(result.Value);
            })
            .WithName("GetAdById")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status200OK)
            .WithTags("Ads")
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Get ad by id",
                Description = "Returns a single advertisement by its unique identifier. " +
                              "Visibility depends on the ad's status and the caller's role: " +
                              "published ads are visible to everyone, draft/paused/archived ads are only visible to the owner, " +
                              "and ads pending moderation are visible to moderators.",
            });
}