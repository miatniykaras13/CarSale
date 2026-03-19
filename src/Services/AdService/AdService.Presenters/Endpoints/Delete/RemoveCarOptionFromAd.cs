using System.Security.Claims;
using AdService.Application.Commands.RemoveCarOptionFromAd;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AdService.Presenters.Endpoints.Delete;

public class RemoveCarOptionFromAd : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapDelete("/ads/{adId:guid}/car-options/{carOptionId:int}", async (
                HttpContext context,
                [FromRoute] Guid adId,
                [FromRoute] int carOptionId,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken ct) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                    return Results.Unauthorized();

                var command = new RemoveCarOptionFromAdCommand(Guid.Parse(userId), adId, carOptionId);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok();
            })
            .RequireAuthorization()
            .WithName("RemoveCarOptionFromAd")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status200OK)
            .WithTags("AdCarOptions")
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Remove car option from ad",
                Description = "Removes the association between a car option and an advertisement. " +
                              "The car option itself is not deleted, only unlinked from the ad. " +
                              "Only the ad owner can perform this action.",
            });
}