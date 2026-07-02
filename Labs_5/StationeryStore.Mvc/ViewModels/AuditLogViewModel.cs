namespace StationeryStore.Mvc.ViewModels;

public class AuditLogViewModel
{
    public string Time { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}