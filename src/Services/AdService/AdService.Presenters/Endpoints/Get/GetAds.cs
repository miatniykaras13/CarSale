using System.Security.Claims;
using AdService.Application.Queries;
using AdService.Application.Queries.GetAds;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AdService.Presenters.Endpoints.Get;

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
            .Produces(StatusCodes.Status200OK)
            .WithTags("Ads")
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Get ads",
                Description = "Returns a paginated list of advertisements with optional filtering. " +
                              "Supports filtering by seller, brand, model, generation, drive type, body type, " +
                              "fuel type, transmission type, and price range. " +
                              "The 'includeImageUrl' query parameter controls whether image URLs are included in the response." +
                              "Ads that are not visible to the current user are not included in the results.",
            });
}