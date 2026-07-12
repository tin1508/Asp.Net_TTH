using StationeryStore.Mvc.Models;
using StationeryStore.Mvc.Data;
using StationeryStore.Mvc.Repositories.RepositoriesConfig;
using Microsoft.EntityFrameworkCore;

namespace StationeryStore.Mvc.Repositories;

public class StationeryOrderRepository : Repository<StationeryOrder, string>, IStationeryOrderRepository
{
    public StationeryOrderRepository(AppDbContext context) : base(context){}
    public override async Task<List<StationeryOrder>> GetAllAsync()
    {
        return await _context.StationeryOrders.AsNoTracking()
                        .Include(o => o.User)
                        .Include(o => o.OrderStationeries)
                        .ThenInclude(od => od.Stationery)
                        .OrderByDescending(o => o.CreatedAt)
                        .ToListAsync();
    }
    public override async Task<StationeryOrder?> GetByIdAsync(string id)
    {
        return await _context.StationeryOrders.AsNoTracking().Include(o => o.OrderStationeries)
                                                                .ThenInclude(od => od.Stationery)
                                                            .FirstOrDefaultAsync(o => o.Id == id);
    }
    public async Task<List<StationeryOrder>> GetByUserIdAsync(string userId)
    {
        return await _context.StationeryOrders.AsNoTracking().Include(o => o.OrderStationeries).ThenInclude(od => od.Stationery).Where(o => o.UserId == userId).OrderByDescending(o => o.CreatedAt).ToListAsync();
    }
}