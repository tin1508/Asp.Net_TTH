using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace StationeryStore.Mvc.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string DefaultShippingAddress { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DateOfBirth { get; set; }
    public ICollection<StationeryOrder> Orders { get; set; } = new List<StationeryOrder>();

}