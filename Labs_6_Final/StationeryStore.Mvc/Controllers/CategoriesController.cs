using StationeryStore.Mvc.ViewModels;
using StationeryStore.Mvc.Models;
using StationeryStore.Mvc.Services;
using Microsoft.AspNetCore.Mvc;
using StationeryStore.Mvc.Exception;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using StationeryStore.Mvc.Mapper;
using AutoMapper;

namespace StationeryStore.Mvc.Controllers;

[Authorize(Policy = "CanViewCategories")]
public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    //inject category service
    public CategoriesController(ICategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllCategoryListAsync();
        var categoryViewModels = categories.Select(c => new CategoryListItemViewModel
        {
            Id = c.Id,
            Name = c.Name,
            StationeryCount = c.Stationeries.Count
        }).ToList();
        return View(categoryViewModels);
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


    [Authorize(Policy = "CanManageCategory")]
    public async Task<IActionResult> CategoryJson()
    {
        var categories = await _categoryService.GetAllCategoryListAsync();
        var responses = _mapper.Map<CategoryListItemViewModel>(categories);
        return Json(responses);
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
    [Authorize(Policy = "CanManageCategory")]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var viewModel = new CategoryCreateViewModel();
        return View(viewModel);
    }
    [Authorize(Policy = "CanManageCategory")]
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