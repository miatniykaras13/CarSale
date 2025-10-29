using AdService.Domain.Ads.Aggregates;
using CSharpFunctionalExtensions;

namespace AdService.Application.Interfaces;

public interface IAdsRepository
{
    Task<Result<Guid>> AddAsync(Ad ad, CancellationToken cancellationToken);

    Task<Result<Guid>> UpdateAsync(Ad ad, CancellationToken cancellationToken);

    Task<Result<Guid>> DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<Result<Ad>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}