namespace StationeryStore.Mvc.ViewModels;

public class CartViewModel
{
    public int CartId {get; set;}
    public List<CartItemListViewModel> Items {get; set;} = new();
    public decimal Total => Items.Sum(i => i.Subtotal);
}