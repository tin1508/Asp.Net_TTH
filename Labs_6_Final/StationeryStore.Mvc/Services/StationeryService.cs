using StationeryStore.Mvc.ViewModels;
using StationeryStore.Mvc.Models;
using Microsoft.EntityFrameworkCore;
using StationeryStore.Mvc.Exception;
using System.Security.Cryptography.X509Certificates;
using StationeryStore.Mvc.Repositories;
using StationeryStore.Mvc.Options;
using Microsoft.Extensions.Options;
using AutoMapper;
using Microsoft.AspNetCore.Components;


public interface IStationeryService
{
    Task<List<Stationery>> GetAllStationeryListAsync();
    Task<Stationery> GetByIdStationery(int id);
    Task<StationeryStatsViewModel> GetStats();
    Task<List<Stationery>> SearchStationery(string? keyword, decimal? minPrice, string? categoryName);
    Task<Stationery> CreateNewStationery(StationeryCreateViewModel model);
    Task<StationeryFilterViewModel> GetFilteredStationeriesAsync(int? categoryId, decimal? minPrice, decimal? maxPrice);
    Task<Stationery> UpdateStationery(int Id, StationeryEditViewModel model);
    Task DeleteStationery(int Id);
    Task<List<Stationery>> StationeryTrash();
    Task<Stationery> RestoreStationery(int Id);
}
public class StationeryService : IStationeryService
{
    // inject my repository
    private readonly IStationeryRepository _stationeryRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly AppSettings _settings;
    private readonly ILogger<StationeryService> _logger;
    private readonly IMapper _mapper;

    public StationeryService(IStationeryRepository stationeryRepository, ICategoryRepository categoryRepository, IOptions<AppSettings> options, ILogger<StationeryService> logger, IMapper mapper)
    {
        _stationeryRepository = stationeryRepository;
        _categoryRepository = categoryRepository;
        _settings = options.Value;
        _logger = logger;
        _mapper = mapper;
    }
    //update existed stationery
    public async Task<Stationery> UpdateStationery(int Id, StationeryEditViewModel model)
    {
        var stationery = await _stationeryRepository.GetByIdAsync(Id) ?? throw new AppException(ErrorCode.NOT_EXISTED_STATIONERY);
        var category = await _categoryRepository.GetCategoryByName(model.CategoryName) ?? throw new AppException(ErrorCode.NOT_EXISTED_CATEGORY);


        _mapper.Map(model, stationery);
        stationery.CategoryId = category.Id;
        stationery.LastUpdatedAt = DateTime.UtcNow;

        try
        {
            await _stationeryRepository.SaveChangeAsync();
            return stationery;
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new AppException(ErrorCode.CONCURRENCY_CONFLICT);
        }
    }

    //get all stationeries list
    public async Task<List<Stationery>> GetAllStationeryListAsync()
    {
        var stationeries = await _stationeryRepository.GetAllAsync();
        return stationeries;
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

    public async Task<List<Stationery>> SearchStationery(string? keyword, decimal? minPrice, string? categoryName)
    {
        if(minPrice.HasValue && minPrice < 0) throw new AppException(ErrorCode.INVALID_PRICE);
        var stationeries = await _stationeryRepository.searchStationeryByKeywordAndMinPrice(keyword, minPrice, categoryName);
        if(stationeries == null || stationeries.Count == 0) throw new AppException(ErrorCode.NOT_FOUND);
        return stationeries;
    }

    public async Task<Stationery> CreateNewStationery(StationeryCreateViewModel model)
    {
        string sku = model.Sku;
        if(sku != null && _stationeryRepository.existedStationery(sku))
        {
            throw new AppException(ErrorCode.EXISTED_STATIONERY);
        }
        var category = await _categoryRepository.GetCategoryByName(model.CategoryName.Trim()) ?? throw new AppException(ErrorCode.NOT_EXISTED_CATEGORY);
        var stationery = _mapper.Map<Stationery>(model);
        stationery.CategoryId = category.Id;
        stationery.LastUpdatedAt = DateTime.UtcNow;
        await _stationeryRepository.AddAsync(stationery);
        Console.WriteLine($"UTC Now: {DateTime.UtcNow}");
        Console.WriteLine($"Local Now: {DateTime.Now}");
        Console.WriteLine($"Server Timezone: {TimeZoneInfo.Local.DisplayName}");
        _logger.LogInformation("Stationery with Sku: {Sku} is created", stationery.Sku);
        await _stationeryRepository.SaveChangeAsync();
        return stationery;
    }
    public async Task DeleteStationery(int Id)
    {
        var stationery = await _stationeryRepository.GetByIdAsync(Id);
        if(stationery == null) throw new AppException(ErrorCode.NOT_EXISTED_STATIONERY);
        stationery.IsDeleted = true;
        stationery.LastUpdatedAt = DateTime.UtcNow;
        stationery.DeletedAt = DateTime.UtcNow;
        await _stationeryRepository.SaveChangeAsync();
        _logger.LogInformation("Stationery with {Id} has been softly deleted.", Id);        
    }
    public async Task<List<Stationery>> StationeryTrash()
    {
        return await _stationeryRepository.GetDeletedStationeries();
    }
    public async Task<Stationery> RestoreStationery(int Id)
    {
        var deletedStationeries = await _stationeryRepository.GetDeletedStationeries();
        var restoreStationery = deletedStationeries.FirstOrDefault(s => s.Id == Id) ??  throw new AppException(ErrorCode.NOT_EXISTED_STATIONERY);
        restoreStationery.DeletedAt = null;
        restoreStationery.LastUpdatedAt = DateTime.UtcNow;
        restoreStationery.IsDeleted = false;
        await _stationeryRepository.SaveChangeAsync();
        _logger.LogInformation("Stationery with {Id} has been just restored", Id);
        return restoreStationery;
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