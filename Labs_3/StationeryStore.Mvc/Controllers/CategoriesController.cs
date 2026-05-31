using StationeryStore.Mvc.ViewModels;
using StationeryStore.Mvc.Models;
using StationeryStore.Mvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace StationeryStore.Mvc.Controllers;

public class CategoriesController : Controller
{
    private readonly CategoryService _categoryService;
    //inject category service
    public CategoriesController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    public IActionResult Index()
    {
        var categories = _categoryService.GetAllCategories()
            .Select(ToListItemViewModel)
            .ToList();
        return View(categories);
    }

    public IActionResult Detail(int id)
    {
        var category = _categoryService.GetById(id);

        if(category == null)
        {
            return NotFound($"Not found category with id: {id}");
        }
        var viewModel = ToDetailViewModel(category);
        return View(viewModel);
    }
    public IActionResult CategoryJson()
    {
        var categories = _categoryService.GetAllCategories()
            .Select(c => new
            {
                c.Id,
                c.Name,
                ProductCount = c.Stationeries.Count
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
    private static CategoryListItemViewModel ToListItemViewModel(Category c)
    {
        return new CategoryListItemViewModel
        {
            Id = c.Id,
            Name = c.Name,
            StationeryCount = c.Stationeries.Count
        };
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
}