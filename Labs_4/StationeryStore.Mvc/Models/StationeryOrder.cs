using System.ComponentModel.DataAnnotations;

namespace StationeryStore.Mvc.Models;
public class StationeryOrder
{
    [Key]
    public string Id {get; set;} = Guid.NewGuid().ToString();
    public string CustomerName {get; set;} = String.Empty;
    public DateTime CreatedAt {get; set;} = DateTime.Now;
    public decimal TotalAmount {get; set;}
    public List<OrderDetail> OrderStationeries {get; set;} = new List<OrderDetail>();
}