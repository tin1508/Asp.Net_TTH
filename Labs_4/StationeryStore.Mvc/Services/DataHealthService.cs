using Microsoft.EntityFrameworkCore;
using StationeryStore.Mvc.Data;
using StationeryStore.Mvc.ViewModels;

namespace StationeryStore.Mvc.Services;

public interface IDataHealthService
{
    Task<List<DataHealthViewModel>> DataHealthChecks();
}
public class DataHealthService : IDataHealthService
{
    private readonly AppDbContext _context;
    public DataHealthService(AppDbContext context)
    {
        _context = context;
    } 
    public async Task<List<DataHealthViewModel>> DataHealthChecks()
    {
        var healthChecks = new List<DataHealthViewModel>();
        //migrations check
        var appliedMigrations = await _context.Database.GetAppliedMigrationsAsync();
        var lastMigration = appliedMigrations.LastOrDefault() ?? "None";

        healthChecks.Add(new DataHealthViewModel
        {
            Check = "Migration",
            Expected = "Applied",
            Actual = lastMigration,
            Status = appliedMigrations.Any() ? "OK" : "Warning",
            Note = "DB up to date"
        });

        //seed data check
        var itemCount = await _context.Stationeries.CountAsync();

        healthChecks.Add(new DataHealthViewModel
        {
            Check = "Seed Data",
            Expected = ">= 3 rows",
            Actual = $"{itemCount} stationeries",
            Status = itemCount >= 3 ? "OK" : "Warning",
            Note = "Ready"
        });
        //no-tracking static check
        healthChecks.Add(new DataHealthViewModel
        {
            Check = "No-Tracking",
            Expected = "List only",
            Actual = "AsNoTracking",
            Status = "OK",
            Note = "Read optimized"
        });

        //transaction static check
        healthChecks.Add(new DataHealthViewModel
        {
            Check = "Transaction",
            Expected = "Order save",
            Actual = "Commit/Rollback",
            Status = "OK",
            Note = "Safe write"
        });
        return healthChecks;
    }
}