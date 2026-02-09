using AutoCatalog.Application.Extensions;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AutoCatalog.Application.BodyTypes.GetBodyTypeById;

public record GetBodyTypeByIdResponse(int Id, string Name);

public class GetBodyTypeByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/body-types/{id:int}", async (
                HttpContext context,
                [FromRoute] int id,
                ISender sender,
                CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetBodyTypeByIdQuery(id), ct);

            if (result.IsFailure)
                return result.Error.ToProblemDetails(context);

            var response = result.Value.Adapt<GetBodyTypeByIdResponse>();
            return Results.Ok(response);
        })
        .WithName("GetBodyTypeById")
        .Produces<GetBodyTypeByIdResponse>(StatusCodes.Status200OK)
        .ProducesGetProblems()
        .WithTags("BodyTypes")
        .WithOpenApi(op => new OpenApiOperation(op) { Summary = "Get body type by id", Description = "Returns body type with given id" });
    }
}