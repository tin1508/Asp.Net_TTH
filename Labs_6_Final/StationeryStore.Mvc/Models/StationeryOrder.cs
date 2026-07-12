using System.ComponentModel.DataAnnotations;
using StationeryStore.Mvc.Enums;

namespace StationeryStore.Mvc.Models;
public class StationeryOrder
{
    [Key]
    public string Id {get; set;} = Guid.NewGuid().ToString();
    public string UserId {get; set;} = string.Empty;
    public ApplicationUser User {get; set;} = null!;
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public decimal TotalAmount {get; set;}
    public OrderStatus Status {get; set;} = OrderStatus.Paid;
    public List<OrderDetail> OrderStationeries {get; set;} = new List<OrderDetail>();
}