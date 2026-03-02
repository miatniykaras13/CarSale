using System.Security.Claims;
using AdService.Application.Queries.GetCarOptionsFromAd;
using AdService.Contracts.Ads.Default;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AdService.Presenters.Endpoints.Get;

public class GetCarOptionsFromAd : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapGet("/ads/{adId:guid}/car-options", async (
                HttpContext context,
                [FromRoute] Guid adId,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken ct) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                var command = new GetCarOptionsFromAdQuery(Guid.Parse(userId ?? Guid.Empty.ToString()), adId);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok(result.Value);
            })
            .WithName("GetCarOptionsFromAd")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<IEnumerable<CarOptionDto>>(StatusCodes.Status200OK);
}