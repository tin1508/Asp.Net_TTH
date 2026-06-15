namespace StationeryStore.Mvc.ViewModels;

public class StationerySelectViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}