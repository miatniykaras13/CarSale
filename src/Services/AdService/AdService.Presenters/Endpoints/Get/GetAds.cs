using System.Security.Claims;
using AdService.Application.Queries;
using AdService.Application.Queries.GetAds;
using AdService.Contracts.Ads.ListItems;
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
                [FromQuery] string sort = GetAdsQuery.Newest,
                CancellationToken ct = default) =>
            {
                Guid? userId = Guid.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var id) &&
                               id != Guid.Empty ? id : null;
                var query = new GetAdsQuery(userId, filter, pageParameters, includeImageUrl, sort);
                var result = await sender.Send(query, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok(result.Value);
            })
            .WithName("GetAds")
            .Produces<AdsPageDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags("Ads")
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Get ads",
                Description = "Returns a paginated list of advertisements with optional filtering. " +
                              "Supports filtering by seller, brand, model, generation, drive type, body type, " +
                              "fuel type, transmission type, and price range. " +
                              "Sort by creation date with 'newest' (default) or 'oldest'. Page sizes range from 1 to 100. " +
                              "The 'includeImageUrl' query parameter controls whether image URLs are included in the response. " +
                              "Total count includes all ads matching the filters and visibility rules. " +
                              "Pages beyond the end return an empty items array and the same total count.",
            });
}
