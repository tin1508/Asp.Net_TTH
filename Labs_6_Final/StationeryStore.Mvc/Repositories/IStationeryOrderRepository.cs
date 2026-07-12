using StationeryStore.Mvc.Models;
using StationeryStore.MvC.Repositories.RepositoriesConfig;

namespace StationeryStore.Mvc.Repositories;

public interface IStationeryOrderRepository : IRepository<StationeryOrder, string>
{
    public Task<List<StationeryOrder>> GetByUserIdAsync(string userId);
}