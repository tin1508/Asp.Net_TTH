using System.Linq;
using StationeryStore.Mvc.ViewModels;
using StationeryStore.Mvc.Models;
using StationeryStore.Mvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StationeryStore.Mvc.Exception;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace StationeryStore.Mvc.Controllers;

[Authorize(Policy = "CanViewStationery")]
public class StationeriesController : Controller
{
    private readonly IStationeryService _stationeryService;
    private readonly ICategoryService _categoryService;
    private readonly IAuditLogService _auditLogService;
    private readonly IMapper _mapper;
    private readonly ICartService _cartService;
    private readonly IOrderStationeryService _orderService;

    //inject stationery service
    public StationeriesController(IStationeryService stationeryService, ICategoryService categoryService, IAuditLogService auditLogService, IMapper mapper, ICartService cartService, IOrderStationeryService orderService)
    {
        _stationeryService = stationeryService;
        _categoryService = categoryService;
        _auditLogService = auditLogService;
        _mapper = mapper;
        _cartService = cartService;
        _orderService = orderService;
    }
    public async Task<IActionResult> Index()
    {
        var stationeries = await _stationeryService.GetAllStationeryListAsync();
        var viewModelList = _mapper.Map<List<StationeryListItemViewModel>>(stationeries);
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
        var viewModel = _mapper.Map<StationeryDetailViewModel>(stationery);
        return View(viewModel);
    }
    [Authorize(Policy = "CanManageStationery")]
    public async Task<IActionResult> Stats()
    {
        var stats = await _stationeryService.GetStats();
        return View(stats);
    }
    [Authorize(Policy = "CanManageStationery")]
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
    [HttpGet]
    public async Task<IActionResult> Search(string? keyword, decimal? minPrice, string? categoryName)
    {
        try
        {
            var stationeries = await _stationeryService.SearchStationery(keyword, minPrice, categoryName);
            var searchViewModelList = _mapper.Map<List<StationeryListItemViewModel>>(stationeries);
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

    [Authorize(Policy = "CanManageStationery")]
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

    [Authorize(Policy = "CanManageStationery")]
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
            await _auditLogService.LogAsync("Create", "Stationery", model.Id.ToString(), "Success", $"Created new stationery: {model.Name} (SKU: {model.Sku})");
            return RedirectToAction(nameof(Index));   
        }
        catch (AppException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            await _auditLogService.LogAsync("Create", "Stationery", model.Id.ToString(), "Failure", $"Failed to create stationery: {model.Name} (SKU: {model.Sku})");
            return View(model);
        }
    }
    [Authorize(Policy = "CanManageStationery")]
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
                CategoryName = stationery.Category?.Name ?? string.Empty,
                Supplier = stationery.Supplier,
                UnitPrice = stationery.Price,
                Quantity = stationery.Quantity,
                MinStock = stationery.MinStock,
            };
            return View(viewmodel); 
        }
        catch(AppException ex)
        {
            TempData["NotFoundMessage"] = ex.Message;
            return NotFound();
        }
    }
    [Authorize(Policy = "CanManageStationery")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, StationeryEditViewModel model)
    {
        if (id != model.Id) return NotFound();
        if (!ModelState.IsValid) return View(model);
        try
        {
            await _stationeryService.UpdateStationery(id, model);
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
    [Authorize(Policy = "CanManageStationery")]
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
    [Authorize(Policy = "CanManageStationery")]
    public async Task<IActionResult> Trash()
    {
        var deletedStationeries = await _stationeryService.StationeryTrash();
        var modelList = _mapper.Map<List<StationeryTrashViewModel>>(deletedStationeries);
        return View(modelList);
    }
    [Authorize(Policy = "CanManageStationery")]
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
    [HttpGet]
    public async Task<IActionResult> BuyNowStationery(int id)
    {
        var stationery = await _stationeryService.GetByIdStationery(id);
        if(stationery is null) return NotFound();
        var model = new BuyNowViewModel
        {
            StationeryId = stationery.Id,
            StationeryName = stationery.Name,
            UnitPrice = stationery.Price
        };
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BuyNowStationery(BuyNowViewModel model)
    {
        if(!ModelState.IsValid) return View(model);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            await _orderService.CreateSingleItemOrder(userId!, model.StationeryId, model.Quantity);
            TempData["SuccessOrder"] = "Đặt hàng thành công!";
            return RedirectToAction("Index", "ProfileUsers");
        }
        catch (AppException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return View(model);
        }
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart(int stationeryId, int quantity = 1)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            await _cartService.AddToCartAsync(userId!, stationeryId, quantity);
            TempData["SuccessMessage"] = "Đã thêm vào giỏ hàng.";
        }
        catch (AppException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index)); 
    }

}