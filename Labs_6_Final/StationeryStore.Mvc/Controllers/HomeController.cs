using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StationeryStore.Mvc.Models;
using StationeryStore.Mvc.Services;
using StationeryStore.Mvc.ViewModels;
using StationeryStore.Mvc.Options;
using Microsoft.AspNetCore.Authorization;

namespace StationeryStore.Mvc.Controllers;

[Authorize(Policy = "CanViewDashboard")]
public class HomeController : Controller
{
    
    private readonly IDashboardService _dashboardService;

    public HomeController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }
    public async Task<IActionResult> Index()
    {
        var model = await _dashboardService.GetDashBoardViewModelAsync();
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
