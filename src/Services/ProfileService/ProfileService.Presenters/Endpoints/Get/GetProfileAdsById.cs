using BuildingBlocks.Extensions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ProfileService.Application.Dtos;
using ProfileService.Application.Queries.GetProfileAdsById;

namespace ProfileService.Presenters.Endpoints.Get;

public class GetProfileAdsById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapGet("/profiles/{id:guid}/ads", async (
                HttpContext context,
                [FromRoute] Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new GetProfileAdsByIdQuery(id), cancellationToken);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok(result.Value);
            })
            .WithName("GetProfileAdsById")
            .WithTags("Profiles")
            .WithSummary("Get a profile's ads by its id")
            .Produces<List<AdSnapshotDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
}
