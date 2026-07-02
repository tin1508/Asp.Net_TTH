using StationeryStore.Mvc.Models;
using StationeryStore.MvC.Repositories.RepositoriesConfig;

namespace StationeryStore.Mvc.Repositories;

public interface IStationeryRepository : IRepository<Stationery, int>
{
    Task<List<Stationery>> searchStationeryByKeywordAndMinPrice(string? keyword, decimal? minPrice, string? categoryName);
    bool existedStationery(string? sku); 
    Task<IEnumerable<Stationery>> GetFilteredAsync(int? categoryId, decimal? minPrice, decimal? maxPrice);
    void SetRowVersion(Stationery stationery, string rowVersion);
    Task<List<Stationery>> GetDeletedStationeries();
}