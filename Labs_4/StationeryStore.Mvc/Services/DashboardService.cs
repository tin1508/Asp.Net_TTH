using StationeryStore.Mvc.Repositories;
using StationeryStore.Mvc.ViewModels;
using StationeryStore.Mvc.Options;
using Microsoft.Extensions.Options;
using StationeryStore.Mvc.Models;

namespace StationeryStore.Mvc.Services;

public interface IDashboardService
{
    Task<DashBoardViewModel> GetDashBoardViewModelAsync();
}
public class DashboardService : IDashboardService
{
    private readonly IStationeryRepository _stationeryRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IStationeryOrderRepository _orderRepository;
   private readonly AppSettings _appSettings;
   private readonly StationeryStoreSettings _storeSettings;

   public DashboardService(IStationeryRepository stationeryRepository,
            ICategoryRepository categoryRepository,
            IStationeryOrderRepository stationeryOrderRepository,
            IOptions<AppSettings> appOptions,
            IOptions<StationeryStoreSettings> storeOptions)
    {
        _stationeryRepository = stationeryRepository;
        _categoryRepository = categoryRepository;
        _orderRepository = stationeryOrderRepository;
        _appSettings = appOptions.Value;
        _storeSettings = storeOptions.Value;
    }

    public async Task<DashBoardViewModel> GetDashBoardViewModelAsync()
    {
        var stationeries = await _stationeryRepository.GetAllAsync();
        var categories = await _categoryRepository.GetAllAsync();
        var orders = await _orderRepository.GetAllAsync();

        int threshold = _storeSettings.LowStockThreshold; 

        var lowStockItems = stationeries
            .Where(s => s.Quantity > 0 && s.Quantity <= threshold)
            .ToList();

        var outOfStockItems = stationeries
            .Where(s => s.Quantity == 0)
            .ToList();

        int totalStationeries = stationeries.Count();
        
        int inStockPercent = totalStationeries == 0 ? 0 : 
            (int)((double)stationeries.Count(s => s.Quantity > (s.MinStock > 0 ? s.MinStock : threshold)) / totalStationeries * 100);

        return new DashBoardViewModel
        {
            TotalStationeries = totalStationeries,
            TotalCategories = categories.Count(),
            TotalOrders = orders.Count(),
            NeedReorderCount = lowStockItems.Count,
            OutOfStockCount = outOfStockItems.Count,
            InStockPercent = inStockPercent,
            Categories = categories.Select(c => new CategoryListItemViewModel
            {
                Id = c.Id,
                Name = c.Name,
                StationeryCount = stationeries.Count(s => s.CategoryId == c.Id)
            }).ToList(), 
            
            LowStockItems = lowStockItems.Select(s => new LowStockStationeryViewModel
            {
                Name = s.Name,
                Quantity = s.Quantity,
            })
            .Concat(outOfStockItems.Select(s => new LowStockStationeryViewModel
            {
                Name = s.Name,
                Quantity = s.Quantity,
            }))
            .ToList()
        };
    }
}