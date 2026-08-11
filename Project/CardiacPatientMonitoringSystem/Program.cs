using CardiacPatientMonitoringSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoringSystem;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Register AppDbContext and configure SQL Server.
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")
            ));

        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Apply pending migrations and seed synthetic data.
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();

            await SeedData.InitializeAsync(context);
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}