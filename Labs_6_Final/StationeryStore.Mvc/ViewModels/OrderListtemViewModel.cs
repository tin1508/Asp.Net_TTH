namespace StationeryStore.Mvc.ViewModels;

public class OrderListItemViewModel
{
    public string Id {get; set;} = String.Empty;
    public string CustomerName {get; set;} = String.Empty;
    public DateTime CreatedAt {get; set;} = DateTime.Now;
    public decimal TotalAmount {get; set;}
    public string Status {get; set;} = string.Empty;
}