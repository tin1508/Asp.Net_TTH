using StationeryStore.Mvc.ViewModels;
using StationeryStore.Mvc.Models;
using StationeryStore.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace StationeryStore.Mvc.Services;

public class CategoryService
{
    private readonly ApplicationDbContext _dbcontext;
    // inject my database context
    public CategoryService(ApplicationDbContext dbContext)
    {
        _dbcontext = dbContext;
    }
    public List<Category> GetAllCategories() {
        return _dbcontext.Categories
            .Include(c => c.Stationeries)
            .ToList();
    }
    public Category? GetById(int id)
    {
        return _dbcontext.Categories
            .Include(c => c.Stationeries)
            .FirstOrDefault(c => c.Id == id);
    }
}