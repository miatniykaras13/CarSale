using System.Security.Claims;
using BuildingBlocks.Extensions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ProfileService.Application.Dtos;
using ProfileService.Application.Queries.GetMyProfile;

namespace ProfileService.Presenters.Endpoints.Get;

public class GetMyProfile : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapGet("/profiles/me", async (
                HttpContext context,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var keycloakId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");

                if (string.IsNullOrWhiteSpace(keycloakId))
                    return Results.Unauthorized();

                var result = await sender.Send(new GetMyProfileQuery(keycloakId), cancellationToken);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok(result.Value);
            })
            .RequireAuthorization()
            .WithName("GetMyProfile")
            .WithTags("Profiles")
            .WithSummary("Get the current user's profile")
            .Produces<MyProfileDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound);
}
