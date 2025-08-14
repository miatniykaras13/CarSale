using CarSale.Contracts;
using CarSale.Contracts.Ads;
using CarSale.Domain.Ads;

namespace CarSale.Application.Ads;

public class AdsService(IAdsRepository adsRepository)
{
    private readonly IAdsRepository _adsRepository = adsRepository;

    public async Task Create(
        CreateAdDto createAdDto,
        CancellationToken cancellationToken)
    {
        var ad = new Ad(
            createAdDto.Title,
            createAdDto.Description,
            createAdDto.
            createAdDto.
        );
    }

    

    public async Task Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
    }

    public async Task Get(
        Guid id,
        CancellationToken cancellationToken)
    {
        
    }
    
}