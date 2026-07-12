namespace StationeryStore.Mvc.ViewModels;

public class CartItemListViewModel
{
    public int Id {get; set;}
    public int StationeryId {get; set;}
    public string StationeryName {get; set;} = string.Empty;
    public decimal UnitPriceSnapshot {get; set;}
    public int Quantity {get; set;}
    public decimal Subtotal => Quantity * UnitPriceSnapshot;
}