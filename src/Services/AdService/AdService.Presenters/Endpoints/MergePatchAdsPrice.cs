using AdService.Application.Commands.MergePatchAdsPrice;
using AdService.Contracts.Ads;
using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AdService.Presenters.Endpoints;

public class MergePatchAdsPrice : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPatch("/ads/{adId:guid}/price", async (
                [FromRoute] Guid adId,
                [FromBody] MoneyMergePatchDto dto,
                ISender sender,
                CancellationToken ct) =>
            {
                var command = new MergePatchAdsPriceCommand(adId, dto);

                var result = await sender.Send(command, ct);
                if (result.IsFailure)
                    return result.Error.ToResponse();


                return Results.Ok();
            })
            .WithName("MergePatchAdsPrice")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status200OK);
}