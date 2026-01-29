using System.Linq.Expressions;
using AdService.Application.Queries;
using AdService.Domain.Aggregates;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AdService.Application.Abstractions.Data;

public interface IAdsRepository
{
    /// <summary>
    /// Gets the ad without navigation properties
    /// </summary>
    /// <param name="id">ad id.</param>
    /// <param name="cancellationToken">cancellation token.</param>
    /// <returns>Result of operation.</returns>
    Task<Result<Ad, Error>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the ad with navigation properties
    /// </summary>
    /// <param name="id">ad id.</param>
    /// <param name="cancellationToken">cancellation token.</param>
    /// <returns>Result of operation.</returns>
    Task<Result<Ad, Error>> GetByIdFullAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<List<Ad>, Error>> GetAllAsync(
        AdFilter filter,
        SortParameters sortParameters,
        PageParameters pageParameters,
        CancellationToken cancellationToken = default);

    Task<Result<List<Ad>, Error>> GetAllFullAsync(
        AdFilter filter,
        SortParameters sortParameters,
        PageParameters pageParameters,
        CancellationToken cancellationToken = default);

    Task<Result<Guid, Error>> AddAsync(Ad entity, CancellationToken cancellationToken = default);

    void Attach(Ad entity);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<Result<Guid, Error>> UpdateAsync(Ad entity, CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}