namespace StationeryStore.Mvc.ViewModels;

public class CategorySearchViewModel
{
    public string Keyword {get; set;} = String.Empty;
    public List<StationeryListItemViewModel> Stationeries {get; set;} = new List<StationeryListItemViewModel>();
}