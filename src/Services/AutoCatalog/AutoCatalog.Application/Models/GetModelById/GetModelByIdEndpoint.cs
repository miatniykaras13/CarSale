using AutoCatalog.Application.Models.GetModelById;
using AutoCatalog.Application.Models.GetModels;
using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.Models.GetModelById;

public record GetModelByIdResponse(int Id, string Name, int BrandId);

public class GetModelByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/models/{id:int}", async ([FromRoute] int id, ISender sender, CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetModelByIdQuery(id), ct);

            if (result.IsFailure)
                return result.ToResponse();

            var response = result.Value.Adapt<GetModelResponse>();
            return Results.Ok(response);
        })
        .WithName("GetModelById")
        .Produces<GetModelByIdResponse>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("Models")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get model by id", Description = "Returns model with given id" });
    }
}