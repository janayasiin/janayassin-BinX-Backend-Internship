using Microsoft.AspNetCore.Identity;

namespace CardiacPatientMonitoringSystem.Data
{
   
    public static class RoleSeedData
    {
        public static async Task InitializeAsync(
            RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Patient"))
            {
                await roleManager.CreateAsync(
                    new IdentityRole("Patient"));
            }
        }
    }
}

