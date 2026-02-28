using System.Security.Claims;
using AdService.Application.Commands.UpdateComment;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AdService.Presenters.Endpoints;

public record UpdateCommentRequest(string Message);

public class UpdateComment : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPut("/ads/{adId:guid}/comment", async (
                HttpContext context,
                [FromRoute] Guid adId,
                [FromBody] UpdateCommentRequest request,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken ct) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                    return Results.Unauthorized();

                var command = new UpdateCommentCommand(Guid.Parse(userId), adId, request.Message);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok();
            })
            .RequireAuthorization()
            .WithName("UpdateComment")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
}

