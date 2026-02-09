using AutoCatalog.Application.Extensions;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Models.GetModels;

public record GetModelResponse(int Id, string Name, int BrandId);

public class GetModelsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/models", async (
                HttpContext context,
                [AsParameters] ModelFilter filter,
                [AsParameters] SortParameters sortParameters,
                [AsParameters] PageParameters pageParameters,
                ISender sender,
                CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetModelsQuery(filter, sortParameters, pageParameters), ct);

            if (result.IsFailure)
                return result.Error.ToProblemDetails(context);

            var response = result.Value.Adapt<List<GetModelResponse>>();
            return Results.Ok(response);
        })
        .WithName("GetModels")
        .Produces<List<GetModelResponse>>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("Models")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get models", Description = "Returns the list of models" });
    }
}