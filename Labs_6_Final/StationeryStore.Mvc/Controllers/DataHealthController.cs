using Microsoft.AspNetCore.Mvc;
using StationeryStore.Mvc.Services;
using Microsoft.AspNetCore.Authorization;

namespace StationeryStore.Mvc.Controllers;

[Authorize(Policy = "CanViewDataHealth")]
public class DataHealthController : Controller
{
    private readonly IDataHealthService _dataHealthService;
    public DataHealthController(IDataHealthService dataHealthService)
    {
        _dataHealthService = dataHealthService;        
    }
    public async Task<IActionResult> Index()
    {
        var model = await _dataHealthService.DataHealthChecks();
        return View(model);
    }
}