namespace Data.Repositories.Contracts;

public interface IRepository<T>
{
    public Task AddAsync(T o);
    public Task Remove(T o);
    public Task Update(T o);
    public Task<T> GetByIdAsync(int id);
    public Task<List<T>> GetAllAsync();
}