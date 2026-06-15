namespace StationeryStore.Mvc.ViewModels;

public class StationerySearchViewModel
{
    public string Keyword {get; set; } = String.Empty;
    public decimal? MinPrice {get; set;}
    public List<StationeryListItemViewModel> Stationeries {get; set;} = new();
}