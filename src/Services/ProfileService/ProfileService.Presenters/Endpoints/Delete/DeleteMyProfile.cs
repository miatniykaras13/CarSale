using System.Security.Claims;
using BuildingBlocks.Extensions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ProfileService.Application.Commands.DeleteMyProfile;

namespace ProfileService.Presenters.Endpoints.Delete;

public class DeleteMyProfile : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapDelete("/profiles/me", async (
                HttpContext context,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var keycloakId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");

                if (string.IsNullOrWhiteSpace(keycloakId))
                    return Results.Unauthorized();

                var result = await sender.Send(new DeleteMyProfileCommand(Guid.Parse(keycloakId)), cancellationToken);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithName("DeleteMyProfile")
            .WithTags("Profiles")
            .WithSummary("Delete the current user's profile")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound);
}
