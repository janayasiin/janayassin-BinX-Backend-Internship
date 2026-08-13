
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("setup")]
        public async Task<IActionResult> Setup()
        {
            // Create the required roles if they don't already exist.
            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));

            // Create a test user and assign the User role.
            var user = await _userManager.FindByEmailAsync("user@test.com");

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = "user@test.com",
                    Email = "user@test.com"
                };

                var result = await _userManager.CreateAsync(user, "User@123");

                if (!result.Succeeded)
                    return BadRequest(result.Errors);
            }

            if (!await _userManager.IsInRoleAsync(user, "User"))
                await _userManager.AddToRoleAsync(user, "User");

            // Create a test admin and assign the Admin role.
            var admin = await _userManager.FindByEmailAsync("admin@test.com");

            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = "admin@test.com",
                    Email = "admin@test.com"
                };

                var result = await _userManager.CreateAsync(admin, "Admin@123");

                if (!result.Succeeded)
                    return BadRequest(result.Errors);
            }

            if (!await _userManager.IsInRoleAsync(admin, "Admin"))
                await _userManager.AddToRoleAsync(admin, "Admin");

            return Ok("Roles and test users are ready.");
        }
    }
}

