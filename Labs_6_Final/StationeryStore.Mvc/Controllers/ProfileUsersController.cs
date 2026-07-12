using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StationeryStore.Mvc.Models;
using StationeryStore.Mvc.Services;
using StationeryStore.Mvc.ViewModels;

namespace StationeryStore.Mvc.Controllers;

[Authorize(Policy = "CanViewProfile")]
public class ProfileUsersController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserService _userService;
    private readonly IOrderStationeryService _orderService;

    public ProfileUsersController(
        UserManager<ApplicationUser> userManager,
        IUserService userService,
        IOrderStationeryService orderService)
    {
        _userManager = userManager;
        _userService = userService;
        _orderService = orderService;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Authentication");
        }

        var profile = await _userService.GetProfileAsync(user.Id);
        if (profile == null)
        {
            return RedirectToAction("Login", "Authentication");
        }

        var isAdmin = User.IsInRole("Admin");

        var viewModel = new ProfilePageViewModel
        {
            Email = user.Email!,
            IsAdmin = isAdmin,
            Profile = profile
        };

        if (!isAdmin)
        {
            viewModel.OrderHistory = await _orderService.GetOrderHistoryByUserIdAsync(user.Id);
        }

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Authentication");

        var profile = await _userService.GetProfileAsync(user.Id);
        if (profile == null) return RedirectToAction("Login", "Authentication");

        return View(profile);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Authentication");

        await _userService.UpdateProfileAsync(user.Id, model);

        TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";
        return RedirectToAction(nameof(Index));
    }
}