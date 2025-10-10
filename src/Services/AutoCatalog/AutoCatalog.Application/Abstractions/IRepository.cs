﻿using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Abstractions;

public interface IRepository<TId, TEntity, in TFilter>
{
    Task<Result<TEntity, Error>> GetByIdAsync(TId id, CancellationToken cancellationToken);

    Task<Result<List<TEntity>, Error>> GetAllAsync(TFilter filter, SortParameters sortParameters, PageParameters pageParameters, CancellationToken cancellationToken);

    Task<Result<TId, Error>> AddAsync(TEntity entity, CancellationToken cancellationToken);

    Task<Result<TId, Error>> UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    Task<Result<Unit, Error>> DeleteAsync(TId id, CancellationToken cancellationToken);
}