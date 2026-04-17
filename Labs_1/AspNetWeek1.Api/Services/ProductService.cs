using AspNetWeek1.Api.Models;
namespace AspNetWeek1.Api.Services;

public class ProductService
{
    private readonly CategoryService _categoryService;
    private readonly List<Product> _products =
    [
        new Product 
        {
            //but chi go faber
            Id = 1, 
            Name = "Faber-Castell 9000 Graphite Pencil (HB)", 
            Price = 80, 
            StockQuantity = 120, 
            CategoryName = "Writing Instruments"
        },
        //but da quang zebra
        new Product { 
            Id = 2, 
            Name = "Zebra Mildliner Highlighter (Set of 5)", 
            Price = 180, 
            StockQuantity = 40, 
            CategoryName = "Writing Instruments"
        },
        //giay sticky note pastel
        new Product { 
            Id = 3, 
            Name = "Pastel Sticky Notes (3x3)", 
            Price = 35, 
            StockQuantity = 75, 
            CategoryName = "Paper & Notebooks"
        },
        new Product { 
            Id = 4, 
            Name = "Standard Metal Stapler", 
            Price = 50, 
            StockQuantity = 40, 
            CategoryName = "Desk Accessories"
        },
        //bo mau nuoc
        new Product{
            Id = 5,
            Name = "Watercolor Paint Set (24 Colors)",
            Price = 120,
            StockQuantity = 30,
            CategoryName = "Art Supplies"
        },
        //So ve phac thao
        new Product{
            Id = 6,
            Name = "Strathmore Sketchbook",
            Price = 22,
            StockQuantity = 10,
            CategoryName = "Art Supplies"
        }
    ];
    // Khoi tao ProductService va gan cac san pham vao cac nhom tuong ung
    public ProductService(CategoryService categoryService)
    {
        _categoryService = categoryService;
        AttachCategoryAllProducts();
    }
    //ham gan tung san pham vao nhom tuong ung
    private void AttachCategoryAllProducts()
    {
        foreach (var product in _products)
        {
            _categoryService.SetCategoryByName(product.CategoryName, product);
        }
    }
    //ham lay tat ca cac san pham
    public List<Product> GetAllProducts() => _products;
    //ham lay so luong ton kho cua san pham dua vao ten san pham
    public int GetStockQuantity(string productName)
    {
        var product = _products.FirstOrDefault(p => p.Name == productName);
        if(product == null)        {
            throw new Exception($"Product with name '{productName}' not found.");
        }
        return product.StockQuantity;
    }
    //ham lay danh muc cua san pham dua vao id san pham
    public string GetCategoryName(int productID)
    {
        var product = _products.FirstOrDefault(p => p.Id == productID);
        if(product == null)        {
            throw new Exception($"Product with ID '{productID}' not found.");
        }
        return product.CategoryName;
    }
}