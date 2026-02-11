using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;
using AdService.Application.Commands.MergePatchAdsCar;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AdService.Presenters.Endpoints;

public class MergePatchAdsCar : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPatch("/ads/{adId:guid}/car", async (
                HttpContext context,
                [FromRoute] Guid adId,
                HttpRequest request,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken ct) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                    return Results.Unauthorized();

                var patchObject =
                    await JsonSerializer.DeserializeAsync<JsonObject>(request.Body, JsonSerializerOptions.Web, ct);

                if (patchObject is null || patchObject.GetType() != typeof(JsonObject))
                    throw new InvalidOperationException("Patch body must be a JsonObject");

                var command = new MergePatchAdsCarCommand(adId, patchObject, Guid.Parse(userId));

                var result = await sender.Send(command, ct);
                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);


                return Results.Ok();
            })
            .RequireAuthorization()
            .WithMetadata(new ConsumesAttribute("application/merge-patch+json"))
            .WithName("MergePatchAdsCar")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status200OK);
}