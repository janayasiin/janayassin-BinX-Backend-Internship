using CardiacPatientMonitoringSystem.Data;
using CardiacPatientMonitoringSystem.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CardiacPatientMonitoringSystem.Day1.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public IConfiguration Configuration { get; private set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] =
                    "test-secret-key-that-is-long-enough-for-hmac",

                ["Jwt:Issuer"] =
                    "CardiacPatientMonitoringSystem",

                ["Jwt:Audience"] =
                    "CardiacPatientMonitoringSystemUsers"
            });

            Configuration = config.Build();
        });

        builder.ConfigureServices(services =>
        {
            using var scope = services
                .BuildServiceProvider()
                .CreateScope();

            var serviceProvider = scope.ServiceProvider;

            var db = serviceProvider
                .GetRequiredService<AppDbContext>();

            var userManager = serviceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            db.Database.EnsureCreated();

            if (!db.Patients.Any())
            {
                var user = new ApplicationUser
                {
                    UserName = "test@example.com",
                    Email = "test@example.com",
                    EmailConfirmed = true,
                    PhoneNumber = "0590000000"
                };

                var result = userManager
                    .CreateAsync(user, "Test123!")
                    .GetAwaiter()
                    .GetResult();

                if (!result.Succeeded)
                {
                    throw new Exception(
                        string.Join(
                            ", ",
                            result.Errors.Select(e => e.Description)));
                }

                userManager
                    .AddToRoleAsync(user, "Patient")
                    .GetAwaiter()
                    .GetResult();

                db.Patients.Add(new Patient
                {
                    UserId = user.Id,
                    FullName = "Test Patient",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Gender = Gender.Male,
                    MedicalHistory = "Test history"
                });

                db.SaveChanges();
            }
        });
    }
}