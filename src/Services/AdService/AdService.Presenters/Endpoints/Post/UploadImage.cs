using System.Security.Claims;
using AdService.Application.Commands.UploadImage;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AdService.Presenters.Endpoints.Post;

public class UploadImage : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost("/ads/{adId:guid}/images", async (
                HttpContext context,
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
                    return result.Error.ToProblemDetails(context);

                return Results.Ok(result.Value);
            })
            .RequireAuthorization()
            .DisableAntiforgery()
            .Accepts<IFormFile>("multipart/form-data")
            .WithName("UploadImage")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status200OK)
            .WithTags("AdImages")
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Upload image to ad",
                Description = "Uploads an image file and attaches it to the specified advertisement. " +
                              "The request must use 'multipart/form-data' content type with the image in the 'file' field. " +
                              "Only the ad owner can upload images. " +
                              "Returns the uploaded image details on success.",
            });
}