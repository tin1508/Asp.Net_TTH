namespace StationeryStore.Mvc.ViewModels;

public class LowStockStationeryViewModel
{
    public string Name { get; set; } = String.Empty;
    public int Quantity { get; set; }
    public string StockStatus => Quantity == 0 ? "detail-danger" : "detail-warning";
}