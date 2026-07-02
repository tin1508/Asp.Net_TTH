using StationeryStore.MvC.Repositories.RepositoriesConfig;
using StationeryStore.Mvc.Data;
using Microsoft.EntityFrameworkCore;

namespace StationeryStore.Mvc.Repositories.RepositoriesConfig;

public class Repository<T, TKey> : IRepository<T, TKey> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    //get all data of entity
    public virtual async Task<List<T>> GetAllAsync() => await _dbSet.AsNoTracking().ToListAsync();

    public virtual async Task<T?> GetByIdAsync(TKey id) => await _dbSet.FindAsync(id).AsTask();

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

    public async Task SaveChangeAsync() => await _context.SaveChangesAsync();
}