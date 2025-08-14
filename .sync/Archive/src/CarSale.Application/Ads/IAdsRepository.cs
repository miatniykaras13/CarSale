using CarSale.Domain.Ads;

namespace CarSale.Application.Ads;

public interface IAdsRepository
{
    Task<Guid> AddAsync(Ad ad, CancellationToken cancellationToken);

    Task<Guid> UpdateAsync(Ad ad, CancellationToken cancellationToken);

    Task<Guid> DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<Ad> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}