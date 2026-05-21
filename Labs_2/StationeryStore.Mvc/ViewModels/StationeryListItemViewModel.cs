using StationeryStore.Mvc.Models;
namespace StationeryStore.Mvc.ViewModels;

public class StationeryListItemViewModel
{
    public int Id { get; set; }
    public string Sku { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public Category? Category{get; set;}
    public string Supplier {get; set;} = String.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public int MinStock { get; set; } = 5;

    public string PriceText => $"{Price:N0} VND";
    public decimal InventoryValue => Price * Quantity;
    public string InventoryValueText => $"{InventoryValue:N0} VND";

    public string StockStatus
    {
        get
        {
            if(Quantity <= 0)
            {
                return "Out of stock";
            }
            if(Quantity <= MinStock)
            {
                return "Need to add more stock";
            }
            return "In stock";
        }
    }
    public string StockStatusClass
    {
        get
        {
            if(Quantity <= 0)
            {
                return "badge badge-danger";
            }
            if(Quantity <= MinStock)
            {
                return "badge badge-warning";
            }
            return "badge badge-success";
        }
    }
}