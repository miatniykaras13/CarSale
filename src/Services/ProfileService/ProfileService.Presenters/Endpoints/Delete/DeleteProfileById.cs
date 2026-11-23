using System.Security.Claims;
using BuildingBlocks.Extensions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ProfileService.Application.Commands.DeleteProfileById;

namespace ProfileService.Presenters.Endpoints.Delete;

public class DeleteProfileById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapDelete("/profiles/{id:guid}", async (
                HttpContext context,
                [FromRoute] Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new DeleteProfileByIdCommand(id), cancellationToken);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                return Results.NoContent();
            })
            .RequireAuthorization("AdminPolicy")
            .WithName("DeleteProfileById")
            .WithTags("Profiles")
            .WithSummary("Delete a profile and its Keycloak account by the local profile id")
            .WithDescription("Only the profile owner can delete it.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);
}
