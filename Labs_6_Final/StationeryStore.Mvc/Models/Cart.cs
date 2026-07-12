using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace StationeryStore.Mvc.Models;

public class Cart
{
    [Key]
    public int Id {get; set;}
    public string UserId {get;set;} = null!;
    public ApplicationUser User {get; set;} = null!;
    public List<CartItem> Items {get; set;} = new();
    public DateTime CreatedDate {get; set;} =  DateTime.UtcNow;
}