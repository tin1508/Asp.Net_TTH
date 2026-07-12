using StationeryStore.Mvc.Repositories;

namespace StationeryStore.Mvc.Configuration;

public static class RepositoryServicesConfig
{
    public static IServiceCollection AddAppRepositories(this IServiceCollection services)
    {
        services.AddScoped<IStationeryRepository, StationeryRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IStationeryOrderRepository, StationeryOrderRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        return services;
    }
}