using CardiacPatientMonitoringSystem.Data;
using CardiacPatientMonitoringSystem.Models;
using Microsoft.AspNetCore.Hosting;
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
                ["Jwt:Key"] = "test-secret-key-that-is-long-enough-for-hmac",
                ["Jwt:Issuer"] = "CardiacPatientMonitoringSystem",
                ["Jwt:Audience"] = "CardiacPatientMonitoringSystemUsers"
            });

            Configuration = config.Build();
        });

        builder.ConfigureServices(services =>
        {
            using var scope = services
                .BuildServiceProvider()
                .CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            db.Database.EnsureCreated();

            if (!db.Patients.Any())
            {
                db.Patients.Add(new Patient
                {
                    FullName = "Test Patient",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Gender = Gender.Male,
                    PhoneNumber = "0590000000",
                    Email = "test@example.com",
                    MedicalHistory = "Test history"
                });

                db.SaveChanges();
            }
        });
    }
}