using CardiacPatientMonitoringSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace CardiacPatientMonitoringSystem.Data;

public static class AdminSeedData
{
    public static async Task InitializeAsync(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        var adminEmail = configuration["Admin:Email"];
        var adminPassword = configuration["Admin:Password"];

        if (string.IsNullOrWhiteSpace(adminEmail) ||
            string.IsNullOrWhiteSpace(adminPassword))
        {
            throw new InvalidOperationException(
                "Admin credentials are not configured.");
        }

        var existingAdmin =
            await userManager.FindByEmailAsync(adminEmail);

        if (existingAdmin != null)
        {
            return;
        }

        var admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(
            admin,
            adminPassword);

        if (!result.Succeeded)
        {
            throw new Exception(
                string.Join(
                    ", ",
                    result.Errors.Select(e => e.Description)));
        }

        var roleResult = await userManager.AddToRoleAsync(
            admin,
            "Admin");

        if (!roleResult.Succeeded)
        {
            throw new Exception(
                string.Join(
                    ", ",
                    roleResult.Errors.Select(e => e.Description)));
        }
    }
}   