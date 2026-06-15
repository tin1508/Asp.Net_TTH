using System.ComponentModel.DataAnnotations;

namespace StationeryStore.Mvc.ViewModels;

public class CategoryCreateViewModel
{
    [Required(ErrorMessage = "Name must be not blank!!!")]
    [StringLength(100, ErrorMessage = "Name must be not exceeded 100 characters")]
    public string Name {get; set;} = String.Empty;
}