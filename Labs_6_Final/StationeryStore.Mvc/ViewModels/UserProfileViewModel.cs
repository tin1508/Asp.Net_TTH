namespace StationeryStore.Mvc.ViewModels;

public class UserProfileViewModel
{
    public string FullName {get; set;} = string.Empty;
    public string DefaultShippingAddress {get; set;} = string.Empty;

    public bool IsActive {get; set;} = true;
    public DateTime? DateOfBirth {get; set;}
}