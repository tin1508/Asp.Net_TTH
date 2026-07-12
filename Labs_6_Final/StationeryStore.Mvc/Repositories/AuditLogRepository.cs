using Microsoft.EntityFrameworkCore;
using StationeryStore.Mvc.Data;
using StationeryStore.Mvc.Models;
using StationeryStore.Mvc.Repositories.RepositoriesConfig;

namespace StationeryStore.Mvc.Repositories;

public class AuditLogRepository : Repository<AuditLog, int>, IAuditLogRepository
{
    public AuditLogRepository(AppDbContext context) : base(context) { }
    
}