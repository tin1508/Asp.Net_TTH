using System.ComponentModel.DataAnnotations;


namespace StationeryStore.Mvc.ViewModels;

public class BuyNowViewModel
{
    public int StationeryId { get; set; }
    public string StationeryName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
    public int Quantity { get; set; } = 1;
}