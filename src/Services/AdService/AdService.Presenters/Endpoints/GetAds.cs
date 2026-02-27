using System.Security.Claims;
using AdService.Application.Queries;
using AdService.Application.Queries.GetAds;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AdService.Presenters.Endpoints;

public class GetAds : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapGet("/ads", async (
                HttpContext context,
                ClaimsPrincipal user,
                [AsParameters] AdFilter filter,
                [AsParameters] PageParameters pageParameters,
                ISender sender,
                [FromQuery] bool includeImageUrl = true,
                CancellationToken ct = default) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                var command = new GetAdsQuery(
                    Guid.Parse(userId ?? Guid.Empty.ToString()),
                    filter,
                    pageParameters,
                    includeImageUrl);
                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok(result.Value);
            })
            .WithName("GetAds")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status200OK);
}