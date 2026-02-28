using System.Security.Claims;
using AdService.Application.Commands.RemoveComment;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AdService.Presenters.Endpoints;

public class RemoveComment : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapDelete("/ads/{adId:guid}/comment", async (
                HttpContext context,
                [FromRoute] Guid adId,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken ct) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                    return Results.Unauthorized();

                var command = new RemoveCommentCommand(Guid.Parse(userId), adId);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok();
            })
            .RequireAuthorization()
            .WithName("RemoveComment")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
}

