using StationeryStore.Mvc.Models;
using StationeryStore.Mvc.Repositories;

namespace StationeryStore.Mvc.Services;

public interface IAuditLogService 
{
    Task LogAsync(string action, string entityName, string? entityId, string result = "Success", string? note = null);
    Task<IEnumerable<AuditLog>> GetAllAuditLogsAsync();
}
public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditLogService(IAuditLogRepository auditLogRepository, IHttpContextAccessor httpContextAccessor)
    {
        _auditLogRepository = auditLogRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task LogAsync(string action, string entityName, string? entityId, string result = "Success", string? note = null)
    {
        var auditLog = new AuditLog
        {
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            UserName = _httpContextAccessor.HttpContext?.User.Identity?.Name,
            IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString(),
            Result = result,
            Note = note,
            CreatedAt = DateTime.UtcNow
        };

        await _auditLogRepository.AddAsync(auditLog);
        await _auditLogRepository.SaveChangeAsync();
    }
    public async Task<IEnumerable<AuditLog>> GetAllAuditLogsAsync()
    {
        return await _auditLogRepository.GetAllAsync();
    }
}