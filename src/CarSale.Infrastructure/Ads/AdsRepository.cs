using CarSale.Application.Ads.Interfaces;
using CarSale.Domain.Ads.Entities;

namespace CarSale.Infrastructure.Ads;

public class AdsRepository : IAdsRepository
{
    private readonly AppDbContext _context;

    public AdsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddAsync(Ad ad, CancellationToken cancellationToken)
    {
        await _context.AddAsync(ad, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return ad.Id;
    }

    public Task<Guid> UpdateAsync(Ad ad, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Guid> DeleteAsync(Guid id, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<Ad> GetByIdAsync(Guid id, CancellationToken cancellationToken) => throw new NotImplementedException();
}