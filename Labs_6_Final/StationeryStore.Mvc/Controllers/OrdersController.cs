using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StationeryStore.Mvc.Services;
using StationeryStore.Mvc.ViewModels;
using StationeryStore.Mvc.Exception;
using Microsoft.Extensions.Validation;
using Microsoft.AspNetCore.Authorization;

namespace StationeryStore.Mvc.Controllers;

[Authorize(Policy = "CanViewOrders")]
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
}