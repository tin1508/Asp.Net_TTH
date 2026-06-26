using System.ComponentModel.DataAnnotations;

namespace StationeryStore.Mvc.ViewModels;

public class OrderItemViewModel
{
    public int StationeryId { get; set; }
    

    [Range(1, int.MaxValue, ErrorMessage = "The quantity must be larger 0!!!")]
    public int Quantity { get; set; } = 1;
}