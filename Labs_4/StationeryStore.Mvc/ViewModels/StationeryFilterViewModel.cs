namespace StationeryStore.Mvc.ViewModels;

public class StationeryFilterViewModel
{
    public int? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

    public List<StationeryListItemViewModel> Results { get; set; } = new List<StationeryListItemViewModel>();
}