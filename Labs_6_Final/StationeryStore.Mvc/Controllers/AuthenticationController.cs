using StationeryStore.Mvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StationeryStore.Mvc.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using StationeryStore.Mvc.Services;
using StationeryStore.Mvc.Configuration;
using Microsoft.AspNetCore.Localization;
using StationeryStore.Mvc.Exception;

namespace StationeryStore.Mvc.Controllers;

[Authorize]
public class AuthenticationController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IAuditLogService _auditLogService;

    public AuthenticationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IAuditLogService auditLogService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _auditLogService = auditLogService;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Forbidden()
    {
        return View(); 
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false); //lockoutOnFailure: preventing brute force attacks by locking the account after a certain number of failed attempts

        if (result.Succeeded)
        {
            await _auditLogService.LogAsync("Login", "Authentication", model.Email, "Success", $"User {model.Email} logged in successfully.");
            if(!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            var isAdmin = user != null && await _userManager.IsInRoleAsync(user, "Admin");

            return isAdmin
                ? RedirectToAction("Index", "Home")
                : RedirectToAction("Index", "Stationeries");
        }
        await _auditLogService.LogAsync("Login", "Authentication", model.Email, "Failure", $"Invalid login attempt for user {model.Email}.");

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public IActionResult GoogleLogin(string provider, string? returnUrl = null)
    {
        if(provider == "Google") provider = AuthSchemes.Google;
        else throw new AppException(ErrorCode.UNKNOWN_PROVIDER);
        // if (returnUrl != null && !Url.IsLocalUrl(returnUrl))
        // {
        //     returnUrl = Url.Content("~/");
        // }
        var redirectUrl = Url.Action("ExternalLoginCallback", "Authentication", new {returnUrl});
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if(info == null)
        {
                return RedirectToAction(nameof(Login));
        }
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        if (result.Succeeded)
        {
                return await RedirectAfterExternalLogin(info, returnUrl);
        }
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        var name = info.Principal.FindFirstValue(ClaimTypes.Name) ?? info.Principal.Identity?.Name;

        if(email != null)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = name ?? "Google User",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow 
                };
                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                await _userManager.AddToRoleAsync(user, "Customer");
            }
            var addLoginResult = await _userManager.AddLoginAsync(user, info);
            if (!addLoginResult.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            return await RedirectAfterExternalLogin(info, returnUrl);
        }
        return RedirectToAction(nameof(Login));
    }
    private async Task<IActionResult> RedirectAfterExternalLogin(ExternalLoginInfo info, string? returnUrl)
    {
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        var user = email != null ? await _userManager.FindByEmailAsync(email) : null;
        var isAdmin = user != null && await _userManager.IsInRoleAsync(user, "Admin");

        return isAdmin
            ? RedirectToAction("Index", "Home")
            : RedirectToAction("Index", "Stationeries");
    }
    [Authorize]
    public IActionResult Secure()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            ModelState.AddModelError(nameof(model.Email), "Email is already registered.");
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FullName = model.FullName,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            
            await _auditLogService.LogAsync(
                "Register",
                "Authentication",
                model.Email,
                "Success",
                $"User {model.Email} registered successfully.");

            await _userManager.AddToRoleAsync(user, "Customer");
            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Stationeries");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        await _auditLogService.LogAsync(
            "Register",
            "Authentication",
            model.Email,
            "Failure",
            string.Join("; ", result.Errors.Select(e => e.Description)));

        return View(model);
    }
}