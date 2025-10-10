using AutoCatalog.Application.Extensions;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Generations.GetGenerationsByModelId;

public record GetGenerationsByModelIdResponse(int Id, string Name);

public class GetGenerationsByModelIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/models/{modelId:int}/generations", async (
                [AsParameters] GenerationFilter filter,
                [AsParameters] SortParameters sortParameters,
                [AsParameters] PageParameters pageParameters,
                [FromRoute] int modelId,
                ISender sender,
                CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetGenerationsByModelIdQuery(filter, sortParameters, pageParameters, modelId), ct);

            if (result.IsFailure)
                return result.ToResponse();

            var response = result.Value.Adapt<List<GetGenerationsByModelIdResponse>>();
            return Results.Ok(response);
        })
        .WithName("GetGenerationsByModelId")
        .Produces<GetGenerationsByModelIdResponse>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("Generations")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get generations by model id", Description = "Returns generations with given model id" });
    }
}