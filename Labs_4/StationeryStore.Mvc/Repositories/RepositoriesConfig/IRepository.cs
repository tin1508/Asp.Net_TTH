namespace StationeryStore.MvC.Repositories.RepositoriesConfig;

public interface IRepository<T, TKey> where T : class
{
    Task<List<T>> GetAllAsync();

    Task<T?> GetByIdAsync(TKey id);

    Task AddAsync(T entity);
    Task SaveChangeAsync();
}