using AdService.Application.Queries;
using AdService.Application.Queries.GetAllCarOptions;
using AdService.Contracts.Ads.Default;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AdService.Presenters.Endpoints.Get;

public class GetAllCarOptions : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapGet("/car-options", async (
                HttpContext context,
                [AsParameters] CarOptionFilter filter,
                [AsParameters] PageParameters pageParameters,
                ISender sender,
                CancellationToken ct) =>
            {
                var query = new GetAllCarOptionsQuery(filter, pageParameters);

                var result = await sender.Send(query, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok(result.Value);
            })
            .WithName("GetAllCarOptions")
            .Produces<IEnumerable<CarOptionDto>>(StatusCodes.Status200OK)
            .WithTags("CarOptions")
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Get all car options",
                Description = "Returns a paginated list of all available car options. " +
                              "Supports filtering by option type (e.g. safety, comfort, multimedia). " +
                              "Does not require authorization.",
            });
}
