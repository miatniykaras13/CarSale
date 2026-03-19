using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;
using AdService.Application.Commands.MergePatchAdsCar;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AdService.Presenters.Endpoints.Patch;

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
            .Produces(StatusCodes.Status200OK)
            .WithTags("Ads")
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Partially update ad's car details",
                Description =
                    "Applies a JSON Merge Patch (RFC 7396) to the car details of the specified advertisement. " +
                    "Content-Type must be 'application/merge-patch+json'. " +
                    "Only the ad owner can perform this action." +
                    "When deleting some fields, dependant fields are also must be deleted. " +
                    "For example, if you delete the 'brandId' field, you must also delete the 'modelId', 'generationId' and other id fields. " +
                    "Fields that don't contain 'Id' are not affected by this rule.",
            });
}