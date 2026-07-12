using Microsoft.AspNetCore.Identity;
using StationeryStore.Mvc.Models;
using StationeryStore.Mvc.ViewModels;
using StationeryStore.Mvc.Exception;

namespace StationeryStore.Mvc.Services;

public interface IUserService
{
    Task<UserProfileViewModel?> GetProfileAsync(string userId);
    Task<UserProfileViewModel> UpdateProfileAsync(string userId, UserProfileViewModel model);
    Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
}

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<UserProfileViewModel?> GetProfileAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return null;

        return new UserProfileViewModel
        {
            FullName = user.FullName,
            DefaultShippingAddress = user.DefaultShippingAddress,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };
    }

    public async Task<UserProfileViewModel> UpdateProfileAsync(string userId, UserProfileViewModel model)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new AppException(ErrorCode.NOT_EXISTED_USER);

        user.FullName = model.FullName;
        user.DefaultShippingAddress = model.DefaultShippingAddress;
        if (model.DateOfBirth.HasValue)
        {
            user.DateOfBirth = DateTime.SpecifyKind(model.DateOfBirth.Value, DateTimeKind.Utc);
        }
        else
        {
            user.DateOfBirth = null;
        }

        await _userManager.UpdateAsync(user);
        return model;
    }

    public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new AppException(ErrorCode.NOT_EXISTED_USER);

        return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }


}