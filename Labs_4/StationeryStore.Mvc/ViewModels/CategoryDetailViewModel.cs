namespace StationeryStore.Mvc.ViewModels;

public class CategoryDetailViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public List<StationeryListItemViewModel> Stationeries { get; set; } = new List<StationeryListItemViewModel>();
}