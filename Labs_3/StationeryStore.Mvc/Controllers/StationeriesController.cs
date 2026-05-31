using StationeryStore.Mvc.ViewModels;
using StationeryStore.Mvc.Models;
using StationeryStore.Mvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StationeryStore.Mvc.Exception;

namespace StationeryStore.Mvc.Controllers;

public class StationeriesController : Controller
{
    private readonly StationeryService _stationeryService;
    //inject stationery service
    public StationeriesController(StationeryService stationeryService)
    {
        _stationeryService = stationeryService;
    }
    public IActionResult Index()
    {
        var stationeries = _stationeryService.GetAllStationeries()
            .Select(ToListItemViewModel)
            .ToList();
        return View(stationeries);
    }
    public IActionResult Detail(int id)
    {
        var stationery = _stationeryService.GetById(id);

        if(stationery == null)
        {
            return NotFound($"Not found stationery with id: {id}");
        }
        var viewModel = ToDetailViewModel(stationery);
        return View(viewModel);
    }
    public IActionResult Stats()
    {
        var stats = _stationeryService.GetStats();
        return View(stats);
    }
    public IActionResult StationeryJson()
    {
        var stationeries = _stationeryService.GetAllStationeries()
            .Select(s => new
            {
                s.Id,
                s.Sku,
                s.Name,
                s.CategoryId,
                s.Price,
                s.Quantity
            });
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
    public IActionResult Search(string? keyword, decimal? minPrice)
    {
        var viewmodel = new StationerySearchViewModel();
        try
        {
            var stationeries = _stationeryService.SearchStationery(keyword, minPrice)
                        .Select(ToListItemViewModel)
                        .ToList();
            viewmodel.Keyword = keyword ?? "";
            viewmodel.MinPrice = minPrice;
            viewmodel.Stationeries = stationeries;
        }
        catch(AppException ex)
        {
            TempData["ErrorMessage"] = ex.Message; 
            
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
    
}