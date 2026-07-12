using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StationeryStore.Mvc.Models;

public class OrderDetail
{
    [Key]
    public string Id {get; set;} = Guid.NewGuid().ToString();

    public string OrderId {get; set;} = String.Empty;

    public StationeryOrder? Order {get; set;}

    public int StationeryId {get; set;}
    public Stationery? Stationery {get; set;}
    public decimal UnitPrice {get; set;}
    public int Quantity {get; set;}

    [NotMapped]
    public decimal SubTotal => UnitPrice * Quantity;
}