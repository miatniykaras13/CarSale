using System.Security.Claims;
using AdService.Application.Commands.AddCarOptionToAd;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AdService.Presenters.Endpoints.Post;

public record AddCarOptionToAdRequest(int CarOptionId);

public class AddCarOptionToAd : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost("/ads/{adId:guid}/car-options", async (
                HttpContext context,
                [FromRoute] Guid adId,
                ClaimsPrincipal user,
                [FromBody] AddCarOptionToAdRequest request,
                ISender sender,
                CancellationToken ct) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                    return Results.Unauthorized();

                var command = new AddCarOptionToAdCommand(Guid.Parse(userId), adId, request.CarOptionId);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok();
            })
            .RequireAuthorization()
            .WithName("AddCarOptionToAd")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status200OK)
            .WithTags("AdCarOptions")
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Add car option to ad",
                Description = "Associates an existing car option with the specified advertisement. " +
                              "The request body must contain the car option identifier. " +
                              "Only the ad owner can add options to their ad.",
            });
}