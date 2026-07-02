using StationeryStore.Mvc.ViewModels;
using StationeryStore.Mvc.Models;
using StationeryStore.Mvc.Data;
using Microsoft.EntityFrameworkCore;
using StationeryStore.Mvc.Repositories;
using StationeryStore.Mvc.Options;
using Microsoft.Extensions.Options;
using StationeryStore.Mvc.Exception;
using System.ComponentModel;

namespace StationeryStore.Mvc.Services;
public interface ICategoryService
{
    Task<List<CategoryListItemViewModel>> GetAllCategoryListAsync();
    Task<Category> GetByIdCategory(int id);
    Task<List<Category>> SearchCategory(string? keyword);
    Task<Category> CreateNewCategory(CategoryCreateViewModel model);
}
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly AppSettings _settings;
    // inject my database context
    public CategoryService(ICategoryRepository categoryRepository, IOptions<AppSettings> options)
    {
        _categoryRepository = categoryRepository;
        _settings = options.Value;
    }
    public async Task<List<CategoryListItemViewModel>> GetAllCategoryListAsync() {
        var categories = await _categoryRepository.GetAllAsync();
        return categories.Select(c => new CategoryListItemViewModel
        {
            Id = c.Id,
            Name = c.Name,
            StationeryCount = c.Stationeries.Count
        }).ToList();
    }
    public async Task<Category> GetByIdCategory(int id)
    {
        var category = await _categoryRepository.GetCategoryById(id);
        if(category == null) throw new AppException(ErrorCode.NOT_EXISTED_CATEGORY);
        return category;
    }
    public async Task<List<Category>> SearchCategory(string? keyword)
    {
        if(String.IsNullOrWhiteSpace(keyword)) throw new AppException(ErrorCode.INVALID_KEY);
        var categories = await _categoryRepository.SearchCategoryByKeyword(keyword);
        if(categories == null) throw new AppException(ErrorCode.NOT_FOUND);
        return categories;
    }
    public async Task<Category> CreateNewCategory(CategoryCreateViewModel model)
    {
        var category = new Category();
        category.Name = model.Name;
        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangeAsync();
        return category;
    }
}