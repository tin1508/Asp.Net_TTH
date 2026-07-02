namespace StationeryStore.Mvc.ViewModels;

public class StationeryEditViewModel : StationeryCreateViewModel
{
    public int Id {get; set;}
    
    public string RowVersion {get; set;} = String.Empty;
}