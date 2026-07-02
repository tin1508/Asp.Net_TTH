using StationeryStore.Mvc.Models;
using StationeryStore.MvC.Repositories.RepositoriesConfig;

namespace StationeryStore.Mvc.Repositories;

public interface IStationeryOrderRepository : IRepository<StationeryOrder, string>
{
    
}