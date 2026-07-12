using System.ComponentModel.DataAnnotations;

namespace StationeryStore.Mvc.ViewModels;

public class StationeryCreateViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Sku must be not blank!!!")]
    [RegularExpression(@"^[A-Z0-9\-]+$", ErrorMessage = "Sku must include capital letters, numbers, and '-'")]
    public string Sku {get; set;} = String.Empty;


    [Required(ErrorMessage =  "Name must be not blank!!!")]
    [StringLength(100, ErrorMessage = "Name must be not exceeded 100 characters!!!")]
    public string Name {get; set;} = String.Empty;

    [Required(ErrorMessage =  "Category must be not blank!!!")]
    public string CategoryName {get; set;} = String.Empty;
    
    [Required(ErrorMessage = "Supplier must be not blank!!!")]
    public string Supplier {get; set;} = String.Empty;
    [Range(1000, 100000000, ErrorMessage = "Price must be from 1.000 to 100.000.000")]
    public decimal UnitPrice {get; set;}

    [Range(0, 10000, ErrorMessage = "Quantity must be from 0 to 10.000")]
    public int Quantity { get; set; }

    [Range(0, 10000, ErrorMessage = "Min stock must be from 0 to 10.000")]
    public int MinStock { get; set; }

}