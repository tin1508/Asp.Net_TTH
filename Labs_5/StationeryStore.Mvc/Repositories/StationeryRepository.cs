using Microsoft.EntityFrameworkCore;
using StationeryStore.Mvc.Data;
using StationeryStore.Mvc.Models;
using StationeryStore.Mvc.Repositories.RepositoriesConfig;

namespace StationeryStore.Mvc.Repositories;

public class StationeryRepository : Repository<Stationery, int>, IStationeryRepository
{
    public StationeryRepository(AppDbContext context) : base(context)
    {
    }
    public async Task<List<Stationery>> searchStationeryByKeywordAndMinPrice(string? keyword, decimal? minPrice, string? categoryName)
    {
        var stationeries = _context.Stationeries.Include(s => s.Category).AsNoTracking().AsQueryable();
        if(keyword != null && keyword != "")
        {
            stationeries = stationeries.Where(s => EF.Functions.ILike(s.Name, $"%{keyword}%")); 
        }
        if (minPrice.HasValue)
        {
            stationeries = stationeries.Where(s => s.Price >= minPrice.Value);
        }
        if (!string.IsNullOrWhiteSpace(categoryName))
        {
            stationeries = stationeries.Where(s => s.Category != null && s.Category.Name == categoryName);
        }
        return await stationeries.ToListAsync();
    }
    public bool existedStationery(string? sku)
    {
        var stationery = _context.Stationeries.Any(s => s.Sku == sku);   
        return stationery;
    }
    public async Task<IEnumerable<Stationery>> GetFilteredAsync(int? categoryId, decimal? minPrice, decimal? maxPrice)
    {
        var query = _context.Stationeries
            .Include(s => s.Category) 
            .AsNoTracking() 
            .AsQueryable();

        if (categoryId.HasValue && categoryId.Value > 0)
        {
            query = query.Where(s => s.CategoryId == categoryId.Value);
        }

        if (minPrice.HasValue)
        {
            query = query.Where(s => s.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(s => s.Price <= maxPrice.Value);
        }

        return await query.ToListAsync();
    }
    public void SetRowVersion(Stationery stationery, string rowVersion)
    {
        _context.Entry(stationery).Property("RowVersion").OriginalValue = Convert.FromBase64String(rowVersion);
    }
    public async Task<List<Stationery>> GetDeletedStationeries()
    {
        var deletedStationeries = await _context.Stationeries.IgnoreQueryFilters().Where(s => s.IsDeleted == true).ToListAsync();
        return deletedStationeries;
    }
    public override Task<Stationery?> GetByIdAsync(int id)
    {
        return _context.Stationeries.Include(s => s.Category).FirstOrDefaultAsync(s => s.Id == id);
    }
}