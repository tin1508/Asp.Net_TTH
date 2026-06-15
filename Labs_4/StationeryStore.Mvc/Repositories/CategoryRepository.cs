using Microsoft.EntityFrameworkCore;
using StationeryStore.Mvc.Data;
using StationeryStore.Mvc.Models;
using StationeryStore.Mvc.Repositories.RepositoriesConfig;

namespace StationeryStore.Mvc.Repositories;

public class CategoryRepository : Repository<Category, string>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context){}
    public async Task<Category?> GetCategoryByName(string? name)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
        return category;
    }
    public async Task<Category?> GetCategoryById(int id)
    {
        var category = await _context.Categories.Include(c => c.Stationeries).FirstOrDefaultAsync(c => c.Id == id);
        return category;
    }
    public async Task<List<Category>> SearchCategoryByKeyword(string? keyword)
    {
        var categories = await _context.Categories.Where(c => EF.Functions.ILike(c.Name, $"%{keyword}%")).AsNoTracking().ToListAsync();
        return categories;
    }
    public override async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories.AsNoTracking().Include(c => c.Stationeries).ToListAsync();
    }
}