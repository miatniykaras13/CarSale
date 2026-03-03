using System.Security.Claims;
using AdService.Application.Commands.CreateCarOption;
using AdService.Domain.Enums;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace AdService.Presenters.Endpoints.Post;

public record CreateCarOptionRequest(OptionType OptionType, string Name, string TechnicalName);

public record CreateCarOptionResponse(int Id);

public class CreateCarOption : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost("/car-options", async (
                HttpContext context,
                ClaimsPrincipal user,
                [FromBody] CreateCarOptionRequest request,
                ISender sender,
                CancellationToken ct) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                    return Results.Unauthorized();

                var command = new CreateCarOptionCommand(request.OptionType, request.Name, request.TechnicalName);

                var result = await sender.Send(command, ct);

                if (result.IsFailure)
                    return result.Error.ToProblemDetails(context);

                CreateCarOptionResponse response = new(result.Value);
                return Results.Created($"/car-options/{response.Id}", response);
            })
            .RequireAuthorization("AdminPolicy")
            .WithName("CreateCarOption")
            .Produces<CreateCarOptionResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status409Conflict)
            .WithTags("CarOptions")
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Create car option",
                Description = "Creates a new car option with the specified type, display name, and technical name. " +
                              "Returns 409 Conflict if a car option with the same technical name already exists. " +
                              "Requires admin privileges.",
            });
}