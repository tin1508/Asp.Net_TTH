using StationeryStore.Mvc.ViewModels;
using StationeryStore.Mvc.Models;
using StationeryStore.AppDbContext;
using Microsoft.EntityFrameworkCore;
using StationeryStore.Mvc.Exception;
using System.Security.Cryptography.X509Certificates;

public class StationeryService
{
    private readonly ApplicationDbContext _dbcontext;
    // inject my database context
    public StationeryService(ApplicationDbContext dbContext)
    {
        _dbcontext = dbContext;
    }
    public List<Stationery> GetAllStationeries()
    {
        return _dbcontext.Stationeries
            .Include(p => p.Category)
            .ToList();
    }
    public Stationery? GetById(int id)
    {
        return _dbcontext.Stationeries
            .Include(p => p.Category)
            .FirstOrDefault(p => p.Id == id);
    }
    public StationeryStatsViewModel GetStats()
    {
        var totalStationeries = _dbcontext.Stationeries.Count();
        var totalQuantity = _dbcontext.Stationeries.Sum(p => p.Quantity);   
        var totalInventoryValue = _dbcontext.Stationeries.Sum(p => p.Price * p.Quantity);
        var outOfStockCount = _dbcontext.Stationeries.Count(p => p.Quantity == 0);
        var needReorderCount = _dbcontext.Stationeries.Count(p => p.Quantity > 0 && p.Quantity <= p.MinStock);
        return new StationeryStatsViewModel
        {
            TotalStationeries = totalStationeries,
            TotalQuantity = totalQuantity,
            TotalInventoryValue = totalInventoryValue,
            OutOfStockCount = outOfStockCount,
            NeedReorderCount = needReorderCount
        };
    }

    public List<Stationery> SearchStationery(string? keyword, decimal? minPrice)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            throw new AppException(ErrorCode.INVALID_KEY);
        if(minPrice < 0)
            throw new AppException(ErrorCode.INVALID_PRICE);
        var stationeries = _dbcontext.Stationeries.Where(s => EF.Functions.ILike(s.Name, $"%{keyword}%") && s.Price >= minPrice).ToList();

        return stationeries;
    }

    public async Task<Stationery> CreateNewStationery(StationeryCreateViewModel model)
    {
        var stationery = new Stationery();
        string sku = model.Sku;
        if(sku != null)
        {
            if(_dbcontext.Stationeries.Any(s => s.Sku == sku))
            {
                throw new AppException(ErrorCode.EXISTED_STATIONERY);
            }
            stationery.Sku = sku;
        }
        var category = _dbcontext.Categories.FirstOrDefaultAsync(c => c.Name == model.CategoryName);
        if(category == null) throw new AppException(ErrorCode.NOT_EXISTED_CATEGORY);
        stationery.Name = model.Name;
        stationery.CategoryId = category.Id;
        stationery.Supplier = model.Supplier;
        stationery.Price = model.UnitPrice;
        stationery.Quantity = model.Quantity;
        stationery.MinStock = model.MinStock;
        stationery.LastUpdatedAt = DateTime.UtcNow;
        _dbcontext.Stationeries.Add(stationery);
        Console.WriteLine($"UTC Now: {DateTime.UtcNow}");
        Console.WriteLine($"Local Now: {DateTime.Now}");
        Console.WriteLine($"Server Timezone: {TimeZoneInfo.Local.DisplayName}");
        await _dbcontext.SaveChangesAsync(); 
        return stationery;
    }

}