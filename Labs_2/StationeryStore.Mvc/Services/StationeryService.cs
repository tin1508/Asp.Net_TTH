using StationeryStore.Mvc.ViewModels;
using StationeryStore.Mvc.Models;
using StationeryStore.AppDbContext;
using Microsoft.EntityFrameworkCore;

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
}