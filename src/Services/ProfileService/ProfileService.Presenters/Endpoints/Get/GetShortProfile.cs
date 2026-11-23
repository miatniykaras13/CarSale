using BuildingBlocks.Extensions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;
using ProfileService.Application.Dtos;
using ProfileService.Application.Queries.GetShortProfile;

namespace ProfileService.Presenters.Endpoints.Get;

public class GetShortProfile : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapGet("/profiles/{id:guid}/short", async (
                HttpContext context,
                [FromRoute] Guid id,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var result = await sender.Send(new GetShortProfileQuery(id), ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok(result.Value);
            })
            .WithName("GetShortProfile")
            .Produces<ShortProfileDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithTags("Profiles")
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Get a short profile by its id",
                Description = "Returns the short version of user profile. Authentication is not required.",
            });
}
