using AutoCatalog.Application.Extensions;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.FuelTypes.GetFuelTypes;

public record GetFuelTypesResponse(int Id, string Name);

public class GetFuelTypesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/fuel-types", async (
                [AsParameters] FuelTypeFilter filter,
                [AsParameters] SortParameters sortParameters,
                [AsParameters] PageParameters pageParameters,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var result = await sender.Send(new GetFuelTypesQuery(filter, sortParameters, pageParameters), ct);

                if (result.IsFailure)
                    return result.Error.ToResponse();

                var response = result.Value.Adapt<List<GetFuelTypesResponse>>();
                return Results.Ok(response);
            })
            .WithName("GetFuelTypes")
            .Produces<List<GetFuelTypesResponse>>(StatusCodes.Status200OK)
            .ProducesGetProblems()
            .WithTags("FuelTypes")
            .WithOpenApi(op =>
                new OpenApiOperation(op) { Summary = "Get fuel types", Description = "Returns the list of fuel types" });
    }
}