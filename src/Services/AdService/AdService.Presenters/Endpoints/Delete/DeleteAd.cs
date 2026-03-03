using System.Security.Claims;
using AdService.Application.Commands.DeleteAd;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AdService.Presenters.Endpoints.Delete;

public class DeleteAd : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapDelete("/ads/{adId:guid}", async (
                HttpContext context,
                [FromRoute] Guid adId,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken ct) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                    return Results.Unauthorized();

                var command = new DeleteAdCommand(adId, Guid.Parse(userId));

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithName("DeleteAd")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags("Ads")
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Delete ad",
                Description = "Deletes an advertisement by its identifier. " +
                              "Only the owner of the ad can perform this action. " +
                              "Ad can be restored after this operation. " +
                              "Returns 204 No Content on success.",
            });
}

