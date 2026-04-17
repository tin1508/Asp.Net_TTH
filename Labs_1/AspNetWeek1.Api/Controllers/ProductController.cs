using AspNetWeek1.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetWeek1.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductController:ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    // endpoint lay tat ca san pham
    [HttpGet("products")]
    public IActionResult GetAllProducts()
    {
        var products = _productService.GetAllProducts()
                    .Select(p => new 
                    {
                        p.Id,
                        p.Name,
                        p.Price,
                        p.StockQuantity,
                        p.CategoryName
                    });
        return Ok(products);
    }
    //endpoint lay ten danh muc cua san pham dua vao id san pham
    [HttpGet("categoryname/{product_id}")]
    public IActionResult GetCategoryName(int product_id)
    {
        try
        {
            var categoryName = _productService.GetCategoryName(product_id);
            return Ok("Category Name: " + categoryName);
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    // endpoint lay so luong ton kho cua san pham dua vao ten san pham
    [HttpGet("stockquantity/{product_name}")]
    public IActionResult GetStockQuantity(string product_name)
    {
        try
        {
            var stockQuantity = _productService.GetStockQuantity(product_name);
            return Ok("Stock Quantity: " + stockQuantity);
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}