namespace StationeryStore.Mvc.ViewModels;

public class StationeryTrashViewModel
{
    public int Id {get; set;}
    public string Sku {get; set;} = String.Empty;
    public string Name {get; set;} = String.Empty;
    public DateTime DeletedAt {get; set;} = DateTime.UtcNow;
}