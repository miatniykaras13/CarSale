using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;
using AdService.Application.Commands.MergePatchAdsCar;
using AdService.Application.Commands.UploadImage;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AdService.Presenters.Endpoints;

public class UploadImage : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost("/ads/{adId:guid}/images", async (
                [FromRoute] Guid adId,
                IFormFile file,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken ct) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                    return Results.Unauthorized();

                var command = new UploadImageCommand(adId, file.OpenReadStream(), file.FileName, file.ContentType, Guid.Parse(userId));

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToResponse();

                return Results.Ok(result.Value);
            })
            .RequireAuthorization()
            .DisableAntiforgery()
            .Accepts<IFormFile>("multipart/form-data")
            .WithName("UploadImage")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status200OK);
}