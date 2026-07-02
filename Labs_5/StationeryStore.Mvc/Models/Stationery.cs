using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StationeryStore.Mvc.Models;

public class Stationery
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } // primary key
    public string Sku { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public int CategoryId { get; set; }
    public Category? Category {get;set;}
    public string Supplier {get; set;} = String.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int MinStock { get; set; } = 5;
    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted {get; set;} = false;
    public DateTime? DeletedAt {get; set;}

    [Timestamp]
    public byte[] RowVersion {get; set;} = Array.Empty<byte>();
    
}