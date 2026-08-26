using CardiacPatientMonitoringSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace CardiacPatientMonitoringSystem.Data;

public static class AdminSeedData
{
    public static async Task InitializeAsync(
        UserManager<ApplicationUser> userManager)
    {
        const string adminEmail = "admin@cardiac.com";
        const string adminPassword = "Admin123!";

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

        await userManager.AddToRoleAsync(
            admin,
            "Admin");
    }
}