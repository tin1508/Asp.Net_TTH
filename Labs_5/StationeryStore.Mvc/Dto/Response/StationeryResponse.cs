using StationeryStore.Mvc.Models;
namespace StationeryStore.Mvc.Dto.Response;
public class StationeryResponse
{
    
    public int Id {get; set;}
    public string Sku { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public int CategoryId { get; set; }
    public Category? Category {get;set;}
    public string Supplier {get; set;} = String.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int MinStock { get; set; } = 5;
    public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
    public bool IsDeleted {get; set;}
    public DateTime DeletedAt {get; set;}
}