using Microsoft.AspNetCore.Identity;
using StationeryStore.Mvc.Models;

namespace StationeryStore.Mvc.Services;

public interface IDataSeedingService
{
    Task SeedAsync();
}
public class DataSeedingService : IDataSeedingService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<DataSeedingService> _logger;
    public DataSeedingService(
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager,
        ILogger<DataSeedingService> logger)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _logger = logger;
    }
    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedAdminUserAsync();
    }
    private async Task SeedRolesAsync()
    {
        string[] roles = { "Admin", "Customer" };
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
                _logger.LogInformation("Seeded role: {Role}", role);
            }
        }
    }
    private async Task SeedAdminUserAsync()
    {
        var adminEmail = "admin@gmail.com";
        var adminPassword = "Admin@123456";

        var adminUser = await _userManager.FindByEmailAsync(adminEmail);

        if (adminUser is null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FullName = "System Administrator"
            };

            var result = await _userManager.CreateAsync(adminUser, adminPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to seed admin user: {Errors}", errors);
                return;
            }

            _logger.LogInformation("Seeded admin user: {Email}", adminEmail);
        }

        if (!await _userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await _userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}