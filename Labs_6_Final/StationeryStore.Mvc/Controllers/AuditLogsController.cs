using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StationeryStore.Mvc.Services;
using StationeryStore.Mvc.ViewModels;

namespace StationeryStore.Mvc.Controllers;

[Authorize(Policy = "CanViewAuditLogs")]
public class AuditLogsController : Controller
{
    private readonly IAuditLogService _auditLogService;

    public AuditLogsController(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    public async Task<IActionResult> Index()
    {
        var auditLogs = await _auditLogService.GetAllAuditLogsAsync();
        var recentLogs = auditLogs
            .OrderByDescending(log => log.CreatedAt)
            .Take(100)
            .Select(log => new AuditLogViewModel
            {
                Time = log.CreatedAt.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss"), // Adjust for UTC to local time if needed
                UserName = log.UserName,
                Action = log.Action,
                EntityName = log.EntityName,
                EntityId = log.EntityId,
                Result = log.Result,
                IpAddress = log.IpAddress
            })
            .ToList();
        return View(recentLogs);
    }
}