using System.Linq;
using StationeryStore.Mvc.ViewModels;
using StationeryStore.Mvc.Models;
using StationeryStore.Mvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StationeryStore.Mvc.Exception;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using StationeryStore.Mvc.Dto.Request;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace StationeryStore.Mvc.Controllers;

public class StationeriesController : Controller
{
    private readonly IStationeryService _stationeryService;
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    //inject stationery service
    public StationeriesController(IStationeryService stationeryService, ICategoryService categoryService, IMapper mapper)
    {
        _stationeryService = stationeryService;
        _categoryService = categoryService;
        _mapper = mapper;
    }
    public async Task<IActionResult> Index()
    {
        var stationeries = await _stationeryService.GetAllStationeryListAsync();
        var viewModelList = stationeries.Select(s => ToListItemViewModel(s)).ToList();
        var categories = await _categoryService.GetAllCategoryListAsync();
        ViewBag.Categories = categories.Select(c => c.Name).Distinct().ToList();
        return View(viewModelList);
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
    public async Task<IActionResult> Search(string? keyword, decimal? minPrice, string? categoryName)
    {
        try
        {
            var stationeries = await _stationeryService.SearchStationery(keyword, minPrice, categoryName);
            var searchViewModelList = stationeries.Select(s => new StationeryListItemViewModel
            {
                Id = s.Id,
                Sku = s.Sku,
                Name = s.Name,
                Category = s.Category,
                Price = s.Price,
                Quantity = s.Quantity,
                MinStock = s.MinStock,

            }).ToList();
            var categories = await _categoryService.GetAllCategoryListAsync();
            ViewBag.Categories = categories.Select(c => c.Name).Distinct().ToList();
            return View("Index", searchViewModelList);
        }catch(AppException ex)
        {
            if(ex.ErrorCode == ErrorCode.INVALID_PRICE)
            {
                TempData["ErrorMessage"] = "The minimum price must be greater than 0!!!";
            }
            else if(ex.ErrorCode == ErrorCode.NOT_FOUND)
            {
                TempData["ErrorMessage"] = "There aren't any stationeries!!!";
            }
            var categories = await _categoryService.GetAllCategoryListAsync();
            ViewBag.Categories = categories.Select(c => c.Name).Distinct().ToList();
            return View("Index", new List<StationeryListItemViewModel>());
        }
        
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
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var stationery = await _stationeryService.GetByIdStationery(id);
        
            var viewmodel = new StationeryEditViewModel()
            {
                Id = stationery.Id,
                Sku = stationery.Sku,
                Name = stationery.Name,
                CategoryName = stationery.Category.Name ?? string.Empty,
                Supplier = stationery.Supplier,
                UnitPrice = stationery.Price,
                Quantity = stationery.Quantity,
                MinStock = stationery.MinStock,
                RowVersion = Convert.ToBase64String(stationery.RowVersion)
            };
            return View(viewmodel); 
        }
        catch(AppException ex)
        {
            TempData["NotFoundMessage"] = ex.Message;
            return NotFound();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, StationeryEditViewModel model)
    {
        if (id != model.Id) return NotFound();
        if (!ModelState.IsValid) return View(model);
        try
        {
            var request = _mapper.Map<StationeryUpdateRequest>(model);
            await _stationeryService.UpdateStationery(id, request);
            return RedirectToAction(nameof(Index));
        }
        catch(AppException ex) when (ex.ErrorCode == ErrorCode.NOT_EXISTED_STATIONERY)
        {
            return NotFound();
        }
        catch(AppException ex) when (ex.ErrorCode == ErrorCode.CONCURRENCY_CONFLICT)
        {
            ModelState.AddModelError(string.Empty,
                                    "Dữ liệu đã được người khác cập nhật. Vui lòng tải lại trang.");
            return View(model);
        }

    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteStationery(int Id)
    {
        try
        {
            await _stationeryService.DeleteStationery(Id);
            TempData["Success"] = "Đã xóa sản phẩm";
            return RedirectToAction(nameof(Index));
        }
        catch (AppException ex)
        {
            TempData["NotFoundMessage"] = ex.Message;
            return NotFound();
        }
    }
    public async Task<IActionResult> Trash()
    {
        var deletedStationeries = await _stationeryService.StationeryTrash();
        var modelList = _mapper.Map<List<StationeryTrashViewModel>>(deletedStationeries);
        return View(modelList);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int Id)
    {
        try
        {
            var stationery = await _stationeryService.RestoreStationery(Id);
            TempData["RestoreSuccessfully"] = "Đã Khôi Phục Văn Phòng Phẩm Thành Công.";
            return RedirectToAction(nameof(Trash));
        }
        catch(AppException ex)
        {
            TempData["NotFoundMessage"] = ex.Message;
            return NotFound();
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