using StationeryStore.Mvc.ViewModels;
using StationeryStore.Mvc.Models;
using StationeryStore.Mvc.Services;
using Microsoft.AspNetCore.Mvc;
using StationeryStore.Mvc.Exception;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace StationeryStore.Mvc.Controllers;

public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;
    //inject category service
    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllCategoryListAsync();
        return View(categories);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var category = await _categoryService.GetByIdCategory(id);

        if(category == null)
        {
            throw new AppException(ErrorCode.NOT_EXISTED_CATEGORY);
        }
        var viewModel = ToDetailViewModel(category);
        return View(viewModel);
    }
    public async Task<IActionResult> CategoryJson()
    {
        var categories = (await _categoryService.GetAllCategoryListAsync())
        .Select(c => new {
            c.Id,
            c.Name,
            ProductCount = c.StationeryCount
        });
        return Json(categories);
    }
    public IActionResult GoToList()
    {
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Force404()
    {
        return NotFound("This is response 404 demo from Force404 action.");
    }
    private static CategoryDetailViewModel ToDetailViewModel(Category c)
    {
        return new CategoryDetailViewModel
        {
            Id = c.Id,
            Name = c.Name,
            Stationeries = c.Stationeries.Select(p => new StationeryListItemViewModel
            {
                Id = p.Id,
                Sku = p.Sku,
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity,
                Supplier = p.Supplier
            }).ToList()
        };
    }
    [HttpGet]
    public async Task<IActionResult> Search(string? keyword)
    {
        var viewmodel = new CategorySearchViewModel();
        try
        {
            var categories = await _categoryService.SearchCategory(keyword);
            viewmodel.Keyword = keyword ?? "";
            viewmodel.Stationeries = categories.SelectMany(c => c.Stationeries)
                                    .Select(s => new StationeryListItemViewModel
                                    {
                                        Id = s.Id,
                                        Sku = s.Sku,
                                        Name = s.Name,
                                        Price = s.Price,
                                        Category = s.Category,
                                        Quantity = s.Quantity,
                                        MinStock = s.MinStock
                                    }).ToList();
        }
        catch(AppException ex)
        {
            TempData["SearchErrorMessage"] = ex.Message;
        }
        return View(viewmodel);
    }
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var viewModel = new CategoryCreateViewModel();
        return View(viewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        try
        {
            await _categoryService.CreateNewCategory(model);
            TempData["SuccessMessage:"] = "Add new category successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch(AppException ex)
        {
            TempData["CreateErrorMessage"] = ex.Message;
            return View(model);
        }
    }
}