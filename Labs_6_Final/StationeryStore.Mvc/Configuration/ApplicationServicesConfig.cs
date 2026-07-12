using StationeryStore.Mvc.Services;

namespace StationeryStore.Mvc.Configuration;

public static class ApplicationServicesConfig
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IStationeryService, StationeryService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IOrderStationeryService, OrderStationeryService>();
        services.AddScoped<IDataHealthService, DataHealthService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IAuditLogService, AuditLogService>();
        services.AddScoped<ICartService, CartService>();
        services.AddTransient<IDataSeedingService, DataSeedingService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}