using System.ComponentModel.DataAnnotations;

namespace StationeryStore.Mvc.ViewModels;

public class OrderCreateViewModel
{
    [Required(ErrorMessage = "Customer name cannot be blank!!!")]
    [MaxLength(100)]
    public string CustomerName {get; set;} = String.Empty;

    public List<OrderItemViewModel> Items { get; set; } = new() { new OrderItemViewModel() };

    public List<StationerySelectViewModel> AvailableStationeries { get; set; } = new List<StationerySelectViewModel>();
}