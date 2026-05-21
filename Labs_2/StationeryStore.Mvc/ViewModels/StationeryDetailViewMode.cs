using StationeryStore.Mvc.Models;

namespace StationeryStore.Mvc.ViewModels;

public class StationeryDetailViewModel
{
    public int Id { get; set; }
    public string Sku { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public Category? Category {get; set;}
    public string Supplier {get; set;} = String.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int MinStock { get; set; } = 5;
    public DateTime LastUpdatedAt { get; set; }

    public string PriceText => $"{Price:N0} VND";
    public decimal InventoryValue => Price * Quantity;
    public string InventoryValueText => $"{InventoryValue:N0} VND";

    public string LastUpdatedAtText => LastUpdatedAt.ToString("dd/MM/yyyy HH:mm");
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
    public string ReorderSuggestion
    {
        get
        {
            if(Quantity <= 0)
            {
                return "Please add more stock because the product is out of stock.";
            }
            if(Quantity <= MinStock)
            {
                return $"Please add more stock. Current quantity is {Quantity}, minimum stock is {MinStock}.";
            }
            return "No need to add more stock.";
        }
    }
}