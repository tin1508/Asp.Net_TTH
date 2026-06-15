using StationeryStore.Mvc.ViewModels;
using StationeryStore.Mvc.Models;
using Microsoft.EntityFrameworkCore;
using StationeryStore.Mvc.Exception;
using System.Security.Cryptography.X509Certificates;
using StationeryStore.Mvc.Repositories;
using StationeryStore.Mvc.Options;
using Microsoft.Extensions.Options;


public interface IStationeryService
{
    Task<List<StationeryListItemViewModel>> GetAllStationeryListAsync();
    Task<Stationery> GetByIdStationery(int id);
    Task<StationeryStatsViewModel> GetStats();
    Task<List<Stationery>> SearchStationery(string? keyword, decimal? minPrice);
    Task<Stationery> CreateNewStationery(StationeryCreateViewModel model);
    Task<StationeryFilterViewModel> GetFilteredStationeriesAsync(int? categoryId, decimal? minPrice, decimal? maxPrice);
}
public class StationeryService : IStationeryService
{
    // inject my repository
    private readonly IStationeryRepository _stationeryRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly AppSettings _settings;
    public StationeryService(IStationeryRepository stationeryRepository, ICategoryRepository categoryRepository, IOptions<AppSettings> options)
    {
        _stationeryRepository = stationeryRepository;
        _categoryRepository = categoryRepository;
        _settings = options.Value;
    }

    //get all stationeries list
    public async Task<List<StationeryListItemViewModel>> GetAllStationeryListAsync()
    {
        var stationeries = await _stationeryRepository.GetAllAsync();
        return stationeries.Select(s => new StationeryListItemViewModel
        {
            Id = s.Id,
            Sku = s.Sku,
            Name = s.Name,
            Price = s.Price,
            Quantity = s.Quantity,
            Category = s.Category
        }).ToList();
    }
    public async Task<Stationery> GetByIdStationery(int id)
    {
        var stationery = await _stationeryRepository.GetByIdAsync(id);
        if(stationery == null) throw new AppException(ErrorCode.NOT_FOUND);
        return stationery;
    }

    public async Task<StationeryStatsViewModel> GetStats()
    {
        var stationeries = await _stationeryRepository.GetAllAsync();
        return new StationeryStatsViewModel
        {
          TotalStationeries = stationeries.Count,
          TotalQuantity = stationeries.Sum(s => s.Quantity),
          TotalInventoryValue = stationeries.Sum(s => s.Quantity*s.Price),
          OutOfStockCount = stationeries.Count(s => s.Quantity == 0),
          NeedReorderCount = stationeries.Count(s => s.Quantity > 0 && s.Quantity <= s.MinStock)  
        };
    }

    public async Task<List<Stationery>> SearchStationery(string? keyword, decimal? minPrice)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            throw new AppException(ErrorCode.INVALID_KEY);
        if(minPrice < 0)
            throw new AppException(ErrorCode.INVALID_PRICE);
        var stationeries = await _stationeryRepository.searchStationeryByKeywordAndMinPrice(keyword, minPrice);
        if(stationeries == null) throw new AppException(ErrorCode.NOT_FOUND);

        return stationeries;
    }

    public async Task<Stationery> CreateNewStationery(StationeryCreateViewModel model)
    {
        var stationery = new Stationery();
        string sku = model.Sku;
        if(sku != null)
        {
            if(_stationeryRepository.existedStationery(sku))
            {
                throw new AppException(ErrorCode.EXISTED_STATIONERY);
            }
            stationery.Sku = sku;
        }
        var category = await _categoryRepository.GetCategoryByName(model.CategoryName.Trim());
        if(category == null) throw new AppException(ErrorCode.NOT_EXISTED_CATEGORY);
        stationery.Name = model.Name;
        stationery.CategoryId = category.Id;
        stationery.Supplier = model.Supplier;
        stationery.Price = model.UnitPrice;
        stationery.Quantity = model.Quantity;
        stationery.MinStock = model.MinStock;
        stationery.LastUpdatedAt = DateTime.UtcNow;
        await _stationeryRepository.AddAsync(stationery);
        Console.WriteLine($"UTC Now: {DateTime.UtcNow}");
        Console.WriteLine($"Local Now: {DateTime.Now}");
        Console.WriteLine($"Server Timezone: {TimeZoneInfo.Local.DisplayName}");
        await _stationeryRepository.SaveChangeAsync();
        return stationery;
    }

    public async Task<StationeryFilterViewModel> GetFilteredStationeriesAsync(int? categoryId, decimal? minPrice, decimal? maxPrice)
    {
        var entities = await _stationeryRepository.GetFilteredAsync(categoryId, minPrice, maxPrice);

        var resultList = entities.Select(e => new StationeryListItemViewModel
        {
            Id = e.Id,
            Sku = e.Sku ?? string.Empty, 
            Name = e.Name,
            Supplier = e.Supplier ?? string.Empty,
            Price = e.Price,
            Quantity = e.Quantity,
            MinStock = e.MinStock,
            Category = e.Category 
        }).ToList();

        return new StationeryFilterViewModel
        {
            CategoryId = categoryId,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            Results = resultList
        };
    }

}