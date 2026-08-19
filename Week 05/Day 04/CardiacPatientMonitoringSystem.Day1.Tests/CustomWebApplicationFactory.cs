using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

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
    }
}