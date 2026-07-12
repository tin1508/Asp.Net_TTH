using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StationeryStore.Mvc.Models;

public class CartItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id{get; set;}
    public int CartId {get; set;}
    public int StationeryId {get; set;}
    public Stationery Stationery {get; set;} = null!;
    public int Quantity {get; set;}
    public decimal UnitPriceSnapshot {get; set;}
}