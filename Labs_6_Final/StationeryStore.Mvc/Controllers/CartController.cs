using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StationeryStore.Mvc.Services;
using StationeryStore.Mvc.Exception;

namespace StationeryStore.Mvc.Controllers;

[Authorize(Policy = "CanViewCart")]
public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cart = await _cartService.GetCartAsync(userId!);
        return View(cart);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int stationeryId, int quantity = 1)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            await _cartService.AddToCartAsync(userId!, stationeryId, quantity);
            TempData["SuccessMessage"] = "Đã thêm vào giỏ hàng.";
        }
        catch (AppException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        try
        {
            await _cartService.UpdateQuantityAsync(userId!, cartItemId, quantity);
        }
        catch (AppException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int cartItemId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        try
        {
            await _cartService.RemoveItemAsync(userId!, cartItemId);
        }
        catch (AppException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            var orderId = await _cartService.CheckoutAsync(userId!);
            TempData["SuccessOrder"] = "Đặt hàng thành công!";
            return RedirectToAction("Index", "ProfileUsers");
        }
        catch (AppException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index)); 
        }
    }
}