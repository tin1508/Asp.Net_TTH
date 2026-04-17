using AspNetWeek1.Api.Models;

namespace AspNetWeek1.Api.Services;

public class CategoryService
{
    private List<Category> _categories =
    [
        new Category
        {
            Id = 1,
            Name = "Writing Instruments"
        },
        new Category
        {
            Id = 2,
            Name = "Paper & Notebooks"
        },
        new Category
        {
            Id = 3,
            Name = "Desk Accessories"
        },
        new Category
        {
            Id = 4,
            Name = "Art Supplies"
        }
    ];
    public void SetCategoryByName(string name, Product product)
    {
        var category = _categories.FirstOrDefault(c => c.Name == name) ?? throw new Exception($"Category with name '{name}' not found.");
        category.Products.Add(product);
    }
    public List<Category> GetAllCategories() => _categories;
}