namespace StationeryStore.Mvc.Configuration;

public static class AuthorizationServicesConfig
{
    public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("CanViewStationery", p => p.RequireRole("Admin", "Customer"));
            options.AddPolicy("CanManageStationery", p => p.RequireRole("Admin"));
            options.AddPolicy("CanViewDashboard", p => p.RequireRole("Admin"));
            options.AddPolicy("CanViewCategories", p => p.RequireRole("Admin", "Customer"));
            options.AddPolicy("CanManageCategory", p => p.RequireRole("Admin"));
            options.AddPolicy("CanViewOrders", p => p.RequireRole("Admin"));
            options.AddPolicy("CanViewAuditLogs", p => p.RequireRole("Admin"));
            options.AddPolicy("CanViewDataHealth", p => p.RequireRole("Admin"));
            options.AddPolicy("CanViewProfile", p => p.RequireRole("Admin", "Customer"));
            options.AddPolicy("CanViewCart", p => p.RequireRole("Customer"));
        });

        return services;
    }
}