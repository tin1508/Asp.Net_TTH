using Microsoft.AspNetCore.Identity;
using StationeryStore.Mvc.Data;
using StationeryStore.Mvc.Models;


namespace StationeryStore.Mvc.Configuration;

public static class IdentityServicesConfig
{
    public static IServiceCollection AddIdentityConfigurations(this IServiceCollection services){
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Authentication/Login";
            options.AccessDeniedPath = "/Authentication/Forbidden";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            options.Cookie.HttpOnly = true;
            options.SlidingExpiration = true;
        });
        return services;
    }
    
}