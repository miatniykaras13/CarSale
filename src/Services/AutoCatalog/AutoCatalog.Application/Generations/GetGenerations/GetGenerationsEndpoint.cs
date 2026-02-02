using AutoCatalog.Application.Extensions;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Generations.GetGenerations;

public record GetGenerationResponse(int Id, string Name, int ModelId, int YearFrom, int YearTo);

public class GetGenerationsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/generations", async (
                HttpContext context,
                [AsParameters] GenerationFilter filter,
                [AsParameters] SortParameters sortParameters,
                [AsParameters] PageParameters pageParameters,
                ISender sender,
                CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetGenerationsQuery(filter, sortParameters, pageParameters), ct);

            if (result.IsFailure)
                return result.Error.ToResponse(context);

            var response = result.Value.Adapt<List<GetGenerationResponse>>();
            return Results.Ok(response);
        })
        .WithName("GetGenerations")
        .Produces<List<GetGenerationResponse>>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("Generations")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get generations", Description = "Returns the list of generations" });
    }
}