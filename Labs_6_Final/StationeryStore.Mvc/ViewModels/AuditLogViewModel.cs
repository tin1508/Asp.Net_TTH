namespace StationeryStore.Mvc.ViewModels;

public class AuditLogViewModel
{
    public string Time { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string Action { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string? EntityId { get; set; }
    public string Result { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
}