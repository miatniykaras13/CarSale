using System.Security.Claims;
using AdService.Application.Commands.GetAdById;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AdService.Presenters.Endpoints;

public class GetAdById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapGet("/ads/{adId:guid}", async (
            [FromRoute] Guid adId,
            ClaimsPrincipal user,
            ISender sender,
            CancellationToken ct = default) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new GetAdByIdCommand(adId, userId is null ? null : Guid.Parse(userId));
            var result = await sender.Send(command, ct);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Results.Ok(result.Value);
        });
}