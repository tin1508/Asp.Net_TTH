using System.Linq;
using StationeryStore.Mvc.ViewModels;
using StationeryStore.Mvc.Models;
using StationeryStore.Mvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StationeryStore.Mvc.Exception;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StationeryStore.Mvc.Controllers;

public class StationeriesController : Controller
{
    private readonly IStationeryService _stationeryService;
    private readonly ICategoryService _categoryService;
    //inject stationery service
    public StationeriesController(IStationeryService stationeryService, ICategoryService categoryService)
    {
        _stationeryService = stationeryService;
        _categoryService = categoryService;
    }
    public async Task<IActionResult> Index()
    {
        var stationeries = await _stationeryService.GetAllStationeryListAsync();
        return View(stationeries);
    }
    public async Task<IActionResult> Detail(int id)
    {
        var stationery = await _stationeryService.GetByIdStationery(id);

        if(stationery == null)
        {
            throw new AppException(ErrorCode.NOT_EXISTED_STATIONERY);
        }
        var viewModel = ToDetailViewModel(stationery);
        return View(viewModel);
    }
    public async Task<IActionResult> Stats()
    {
        var stats = await _stationeryService.GetStats();
        return View(stats);
    }
    public async Task<IActionResult> StationeryJson()
    {
        var stationeries = await _stationeryService.GetAllStationeryListAsync();
        return Json(stationeries);
    }
    public IActionResult GoToList()
    {
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Force404()
    {
        return NotFound("This is response 404 demo from Force404 action.");
    }
    private static StationeryListItemViewModel ToListItemViewModel(Stationery stationery)
    {
        return new StationeryListItemViewModel
        {
            Id = stationery.Id,
            Sku = stationery.Sku,
            Name = stationery.Name,
            Price = stationery.Price,
            Category = stationery.Category,
            Quantity = stationery.Quantity,
            MinStock = stationery.MinStock
        };
    }
    private static StationeryDetailViewModel ToDetailViewModel(Stationery stationery)
    {
        return new StationeryDetailViewModel
        {
            Id = stationery.Id,
            Sku = stationery.Sku,
            Name = stationery.Name,
            Price = stationery.Price,
            Category = stationery.Category,
            Supplier = stationery.Supplier,    
            Quantity = stationery.Quantity,
            MinStock = stationery.MinStock,
            LastUpdatedAt = stationery.LastUpdatedAt
        };
    }
    [HttpGet]
    public async Task<IActionResult> Search(string? keyword, decimal? minPrice)
    {
        var viewmodel = new StationerySearchViewModel();
        try
        {
            var stationeries = await _stationeryService.SearchStationery(keyword, minPrice);
            viewmodel.Keyword = keyword ?? "";
            viewmodel.MinPrice = minPrice;
            viewmodel.Stationeries = stationeries.Select(ToListItemViewModel).ToList();
        }
        catch(AppException ex)
        {
            TempData["SearchErrorMessage"] = ex.Message; 
            
        }
        return View(viewmodel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var viewModel = new StationeryCreateViewModel
        {
          Quantity = 1,
          MinStock = 1  
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StationeryCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        try
        {
            await _stationeryService.CreateNewStationery(model); 
            TempData["SuccessMessage:"] = "Add new stationery successfully!!!";  
            return RedirectToAction(nameof(Index));   
        }
        catch (AppException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Filter(int? categoryId, decimal? minPrice, decimal? maxPrice)
    {
        var model = await _stationeryService.GetFilteredStationeriesAsync(categoryId, minPrice, maxPrice);

        var categories = await _categoryService.GetAllCategoryListAsync();
        
        ViewBag.CategoryList = new SelectList(categories, "Id", "Name", categoryId);

        return View(model);
    }
    
}