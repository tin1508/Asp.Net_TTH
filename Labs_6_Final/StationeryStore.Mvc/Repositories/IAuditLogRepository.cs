using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using StationeryStore.Mvc.Models;
using StationeryStore.MvC.Repositories.RepositoriesConfig;

namespace StationeryStore.Mvc.Repositories;

public interface IAuditLogRepository : IRepository<AuditLog, int>
{

}