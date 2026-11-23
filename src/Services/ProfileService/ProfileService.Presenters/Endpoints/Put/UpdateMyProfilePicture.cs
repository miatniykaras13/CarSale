using System.Security.Claims;
using BuildingBlocks.Extensions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ProfileService.Application.Commands.UpdateMyProfilePicture;
using ProfileService.Application.Dtos;

namespace ProfileService.Presenters.Endpoints.Put;

public class UpdateMyProfilePicture : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPut("/profiles/me/picture", async (
                HttpContext context,
                ClaimsPrincipal user,
                IFormFile file,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var subject = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");
                if (!Guid.TryParse(subject, out var keycloakId) || keycloakId == Guid.Empty)
                    return Results.Unauthorized();

                using var stream = file.OpenReadStream();
                var command = new UpdateMyProfilePictureCommand(
                    keycloakId, stream, file.FileName, file.ContentType);
                var result = await sender.Send(command, cancellationToken);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok(new UpdateProfilePictureResponse(result.Value));
            })
            .RequireAuthorization()
            .DisableAntiforgery()
            .Accepts<IFormFile>("multipart/form-data")
            .WithName("UpdateMyProfilePicture")
            .WithTags("Profiles")
            .WithSummary("Replace the current user's profile picture")
            .WithDescription("Upload an image in the multipart/form-data field named file.")
            .Produces<UpdateProfilePictureResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
}