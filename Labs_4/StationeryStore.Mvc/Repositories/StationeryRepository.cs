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
    public async Task<List<Stationery>> searchStationeryByKeywordAndMinPrice(string? keyword, decimal? minPrice)
    {
        var stationeries = await _context.Stationeries.Where(s => EF.Functions.ILike(s.Name, $"%{keyword}%") && s.Price >= minPrice).AsNoTracking().ToListAsync();
        return stationeries;
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
}