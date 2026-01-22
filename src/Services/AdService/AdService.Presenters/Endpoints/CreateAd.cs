using System.Security.Claims;
using AdService.Application.Commands.CreateAd;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AdService.Presenters.Endpoints;

public class CreateAd : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost("/ads", async (ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                    return Results.Unauthorized();

                var result = await sender.Send(new CreateAdCommand(Guid.Parse(userId)), ct);

                if (result.IsFailure)
                    return result.Error.ToResponse();

                var response = result.Value;

                if (response.AdExisted)
                    return Results.Ok(response.AdId);


                return Results.Created($"/ads/{response.AdId}", response);
            })
            .RequireAuthorization()
            .WithName("CreateAd")
            .Produces<CreateAdResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<CreateAdResponse>(StatusCodes.Status200OK);
}