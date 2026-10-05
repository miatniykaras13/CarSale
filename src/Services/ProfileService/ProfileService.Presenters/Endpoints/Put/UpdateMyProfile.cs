using System.Security.Claims;
using BuildingBlocks.Extensions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ProfileService.Application.Commands.UpdateMyProfile;
using ProfileService.Application.Dtos;

namespace ProfileService.Presenters.Endpoints.Put;

public class UpdateMyProfile : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPut("/profiles/me", async (
                HttpContext context,
                ClaimsPrincipal user,
                [FromBody] UpdateMyProfileDto profile,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var subject = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");

                if (!Guid.TryParse(subject, out var keycloakId) || keycloakId == Guid.Empty)
                    return Results.Unauthorized();

                var result = await sender.Send(new UpdateMyProfileCommand(keycloakId, profile), cancellationToken);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithName("UpdateMyProfile")
            .WithTags("Profiles")
            .WithSummary("Replace the current user's editable profile fields")
            .WithDescription("Username, email, name, surname and phoneNumber are required. " +
                             "Picture is cleared when omitted or null.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
}
