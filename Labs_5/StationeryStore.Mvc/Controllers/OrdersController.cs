using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StationeryStore.Mvc.Services;
using StationeryStore.Mvc.ViewModels;
using StationeryStore.Mvc.Exception;
using Microsoft.Extensions.Validation;

namespace StationeryStore.Mvc.Controllers;

public class OrdersController : Controller
{
    private readonly IOrderStationeryService _orderService;
    private readonly IStationeryService _stationeryService;

    public OrdersController(IOrderStationeryService orderService, IStationeryService stationeryService)
    {
        _orderService = orderService;
        _stationeryService = stationeryService;
    }

    public async Task<IActionResult> Index()
    {
        var orders = await _orderService.GetAllOrderListAsync();
        return View(orders);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var stationeries = await _stationeryService.GetAllStationeryListAsync();
        var viewModel = new OrderCreateViewModel
        {
            AvailableStationeries = stationeries.Select(s => new StationerySelectViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.Price,
                Stock = s.Quantity
            }).ToList()
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(OrderCreateViewModel model)
    {
        Console.WriteLine($"Items count: {model.Items?.Count ?? -1}");
        foreach (var item in model.Items ?? new())
            Console.WriteLine($"StationeryId = {item.StationeryId}, Qty={item.Quantity}");
        if (!ModelState.IsValid)
        {
            // Reload dropdown khi validation fail
            var stationeries = await _stationeryService.GetAllStationeryListAsync();
            model.AvailableStationeries = stationeries.Select(s => new StationerySelectViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.Price,
                Stock = s.Quantity
            }).ToList();
            return View(model);
        }

        try
        {
            await _orderService.CreateNewOrder(model);
            TempData["SuccessMessage"] = "Tạo đơn hàng thành công!";
            return RedirectToAction("Index");
        }
        catch (AppException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return View(model);
        }
    }
}