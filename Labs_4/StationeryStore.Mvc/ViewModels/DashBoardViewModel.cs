namespace StationeryStore.Mvc.ViewModels;

public class DashBoardViewModel
{
    public int TotalStationeries {get; set;}
    public int TotalCategories {get; set;}
    public int NeedReorderCount {get; set;}
    public int OutOfStockCount {get; set;}

    public int TotalOrders {get; set;}
    public int InStockPercent {get; set;}

    public List<CategoryListItemViewModel> Categories {get; set;} = new List<CategoryListItemViewModel>();
    public List<LowStockStationeryViewModel> LowStockItems {get; set;} = new List<LowStockStationeryViewModel>();
}

