using AdService.Application.Commands.MergePatchAdsCar;
using AdService.Application.Commands.MergePatchAdsLocation;
using AdService.Contracts.Ads;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AdService.Presenters.Endpoints;

public class MergePatchAdsLocation : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPatch("/ads/{adId:guid}/location", async (
                [FromRoute] Guid adId,
                [FromBody] LocationMergePatchDto dto,
                ISender sender,
                CancellationToken ct) =>
            {
                var command = new MergePatchAdsLocationCommand(adId, dto);

                var result = await sender.Send(command, ct);
                if (result.IsFailure)
                    return result.Error.ToResponse();


                return Results.Ok();
            })
            .WithName("MergePatchAdsLocation")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status200OK);
}