namespace StationeryStore.Mvc.ViewModels;
public class OrderDetailViewModel
{
    public string TransactionCode { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int TotalItems { get; set; }
}