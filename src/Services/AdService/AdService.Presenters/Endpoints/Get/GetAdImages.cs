using System.Security.Claims;
using AdService.Application.Queries.GetAdImages;
using AdService.Contracts.Ads.Default;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AdService.Presenters.Endpoints.Get;

public class GetAdImages : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapGet("/ads/{adId:guid}/images", async (
                HttpContext context,
                [FromRoute] Guid adId,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken ct) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                var query = new GetAdImagesQuery(adId, userId is null ? null : Guid.Parse(userId));

                var result = await sender.Send(query, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok(result.Value);
            })
            .WithName("GetAdImages")
            .Produces<IEnumerable<AdImageDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
}

