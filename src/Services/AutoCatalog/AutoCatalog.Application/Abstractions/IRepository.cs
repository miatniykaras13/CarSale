namespace AutoCatalog.Application.Abstractions;

public interface IRepository<TId, TEntity>
{
    Task<Result<TEntity, Error>> GetByIdAsync(TId id, CancellationToken cancellationToken);

    Task<Result<List<TEntity>, Error>> GetAllAsync(CancellationToken cancellationToken);

    Task<Result<TId, Error>> AddAsync(TEntity entity, CancellationToken cancellationToken);

    Task<Result<TId, Error>> UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    Task<Result<Unit, Error>> DeleteAsync(TId id, CancellationToken cancellationToken);
}