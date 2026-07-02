using Microsoft.AspNetCore.Mvc;
using StationeryStore.Mvc.ViewModels;

namespace StationeryStore.Mvc.Controllers;

public class AuditLogsController : Controller
{
    public IActionResult Index()
    {
        var logs = new List<AuditLogViewModel>();
        var logDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");

        if (Directory.Exists(logDir))
        {
            var latestFile = Directory.GetFiles(logDir, "lab05-*.txt")
                .OrderByDescending(f => f)
                .FirstOrDefault();

            if (latestFile != null)
            {
                using var stream = new FileStream(latestFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(stream);
                var content = reader.ReadToEnd();
                var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var line in lines)
                {
                    var log = ParseLogLine(line);
                    if (log != null) logs.Add(log);
                }
            }
        }

        logs.Reverse();
        return View(logs);
    }

    private AuditLogViewModel? ParseLogLine(string line)
    {
        try
        {
            var parts = line.Split(' ', 4);
            if (parts.Length < 4) return null;

            var date = parts[0];
            var time = parts[1].Split('.')[0]; 
            var level = parts[3].Contains("[INF]") ? "Information"
                      : parts[3].Contains("[WRN]") ? "Warning"
                      : parts[3].Contains("[ERR]") ? "Error"
                      : null;

            if (level == null) return null;

            var message = line[(line.IndexOf(']') + 2)..];

            return new AuditLogViewModel
            {
                Time = $"{DateTime.Parse(date):dd/MM} {time}",
                Level = level,
                Message = message.Length > 80 ? message[..80] + "..." : message
            };
        }
        catch
        {
            return null;
        }
    }
}