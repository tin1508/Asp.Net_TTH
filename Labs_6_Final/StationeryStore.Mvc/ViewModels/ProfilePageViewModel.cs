namespace StationeryStore.Mvc.ViewModels;

public class ProfilePageViewModel
{
    public string Email { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public UserProfileViewModel Profile { get; set; } = null!;
    public List<OrderListItemViewModel> OrderHistory { get; set; } = new();
}