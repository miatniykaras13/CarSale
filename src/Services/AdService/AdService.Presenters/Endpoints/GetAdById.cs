using AdService.Application.Commands.GetPublishedAdById;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AdService.Presenters.Endpoints;

public class GetAdById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapGet("/ads/{adId:guid}", async ([FromRoute] Guid adId, ISender sender, CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetPublishedAdByIdCommand(adId), ct);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Results.Ok(result.Value);
        });
}