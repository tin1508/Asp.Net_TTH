using AspNetWeek1.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetWeek1.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CategoryController:ControllerBase
{
    private readonly CategoryService _categoryService;
    private readonly ProductService _productService;

    public CategoryController(CategoryService categoryService, ProductService productService)
    {
        _categoryService = categoryService;
        _productService = productService;
    }

    //endpoint lay tat ca danh muc va thong tin tat ca san pham trong tung danh muc
    [HttpGet("categories")]
    public IActionResult GetAllCategories()
    {
        var categories = _categoryService.GetAllCategories();
        return Ok(categories);
    }
}