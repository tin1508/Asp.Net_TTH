using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using StationeryStore.Mvc.Models;
using StationeryStore.MvC.Repositories.RepositoriesConfig;

namespace StationeryStore.Mvc.Repositories;

public interface ICategoryRepository : IRepository<Category, string>
{
    Task<Category?> GetCategoryByName(string? name);
    Task<Category?> GetCategoryById(int id);
    Task<List<Category>> SearchCategoryByKeyword(string? keyword);
    
}