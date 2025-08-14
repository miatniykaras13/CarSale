using CarSale.Contracts;
using CarSale.Contracts.Ads;
using CarSale.Contracts.Cars;
using Microsoft.AspNetCore.Mvc;

namespace CarSale.Presenters.Controllers;

// Post /ads/Car
// Get /ads/{id}/transport
// Get /ads/{id}/user
// Get ads/{id}/similar-ads
// Delete /Ad/{id}
// Patch /Ad/{id}
// 

[ApiController]
[Route("api/[controller]")]
public class AdsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateAdDto adDto,
        CancellationToken cancellationToken = default)
    {
        await Task.Delay(5000, cancellationToken);
        return Ok("Ad created");
    }

    [HttpPost]
    [Route("{adId:guid}/transport")]
    public async Task<IActionResult> AddTransport(
        [FromRoute] Guid adId,
        [FromBody] CreateCarDto carDto,
        CancellationToken cancellationToken)
    {
        return Ok("Transport");
    }

    [HttpGet]
    [Route("{adId:guid}")]
    public async Task<IActionResult> Get(
        [FromRoute] Guid adId,
        CancellationToken cancellationToken)
    {
        return Ok("Ad with id");
    }

    [HttpGet]
    [Route("{adId:guid}/transport")]
    public async Task<IActionResult> GetTransport(
        [FromRoute] Guid adId,
        CancellationToken cancellationToken)
    {
        return Ok("Transport");
    }

    [HttpGet]
    [Route("{adId:guid}/similar-ads")]
    public async Task<IActionResult> GetSimilarAds(
        [FromRoute] Guid adId,
        CancellationToken cancellationToken)
    {
        return Ok("similar-ads");
    }

    [HttpGet]
    [Route("{adId:guid}/seller")]
    public async Task<IActionResult> GetSeller(
        [FromRoute] Guid adId,
        CancellationToken cancellationToken)
    {
        return Ok("Ad with id");
    }

    [HttpDelete]
    [Route("{adId:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid adId,
        CancellationToken cancellationToken)
    {
        return Ok("Ad deleted");
    }

    [HttpPatch]
    [Route("{adId:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid adId,
        [FromBody] UpdateAdDto updateAdDto,
        CancellationToken cancellationToken)
    {
        return Ok("Ad with id");
    }

}