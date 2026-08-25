
using EventHub.Data;
using EventHub.Services;
using EventHub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<IEventService, EventService>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            // Configure Entity Framework Core to use SQL Server.
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
