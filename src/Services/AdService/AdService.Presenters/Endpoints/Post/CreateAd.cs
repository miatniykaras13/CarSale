using System.Security.Claims;
using AdService.Application.Commands.CreateAd;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AdService.Presenters.Endpoints.Post;

public class CreateAd : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost("/ads", async (
                HttpContext context,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken ct) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                    return Results.Unauthorized();

                var result = await sender.Send(new CreateAdCommand(Guid.Parse(userId)), ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                var response = result.Value;

                if (response.AdExisted)
                    return Results.Ok(response);

                return Results.CreatedAtRoute($"/ads/{response.AdId}", response);
            })
            .RequireAuthorization()
            .WithName("CreateAd")
            .Produces<CreateAdResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<CreateAdResponse>(StatusCodes.Status200OK)
            .WithTags("Ads")
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Create ad",
                Description = "Creates a new advertisement draft for the authenticated user. " +
                              "If the user already has an existing draft, returns 200 OK with the existing draft's id. " +
                              "Otherwise, creates a new ad and returns 201 Created with the new ad id. " +
                              "The seller snapshot is automatically created from the provided token.",
            });
}