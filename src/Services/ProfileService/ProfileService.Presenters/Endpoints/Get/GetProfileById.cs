using BuildingBlocks.Extensions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ProfileService.Application.Dtos;
using ProfileService.Application.Queries.GetProfileById;

namespace ProfileService.Presenters.Endpoints.Get;

public class GetProfileById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapGet("/profiles/{id:guid}", async (
                HttpContext context,
                [FromRoute] Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new GetProfileByIdQuery(id), cancellationToken);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.Ok(result.Value);
            })
            .WithName("GetProfileById")
            .WithTags("Profiles")
            .WithSummary("Get a profile by its id")
            .Produces<ProfileByIdDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound);
}
