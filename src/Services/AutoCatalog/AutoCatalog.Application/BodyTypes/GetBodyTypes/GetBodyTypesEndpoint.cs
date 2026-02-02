using AutoCatalog.Application.Extensions;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.BodyTypes.GetBodyTypes;

public record GetBodyTypesResponse(int Id, string Name);

public class GetBodyTypesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/body-types", async (
                HttpContext context,
                [AsParameters] BodyTypeFilter filter,
                [AsParameters] SortParameters sortParameters,
                [AsParameters] PageParameters pageParameters,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var result = await sender.Send(new GetBodyTypesQuery(filter, sortParameters, pageParameters), ct);

                if (result.IsFailure)
                    return result.Error.ToResponse(context);

                var response = result.Value.Adapt<List<GetBodyTypesResponse>>();
                return Results.Ok(response);
            })
            .WithName("GetBodyTypes")
            .Produces<List<GetBodyTypesResponse>>(StatusCodes.Status200OK)
            .ProducesGetProblems()
            .WithTags("BodyTypes")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Get body types", Description = "Returns the list of body types" });
    }
}