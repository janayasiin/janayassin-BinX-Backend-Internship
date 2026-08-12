using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyFirstApi.Data;
using MyFirstApi.Middleware;
using MyFirstApi.Validators;
using System.Text;

namespace MyFirstApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers + FluentValidation
            builder.Services

                .AddControllers()
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<CreateBookRequestValidator>();
                });

            // Swagger
            builder.Services.AddSwaggerGen();

            // Database
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));

            // ASP.NET Core Identity
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            // Authorization Policy
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("CanManageBooks", policy =>
                    policy.RequireClaim("Permission", "ManageBooks"));
            });

            // JWT Authentication
            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme =
                        JwtBearerDefaults.AuthenticationScheme;

                    options.DefaultChallengeScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                builder.Configuration["Jwt:Key"]!
                            )
                        )
                    };
                });

            var app = builder.Build();

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI();

            // Custom middleware
            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseHttpsRedirection();

            // Authentication must come before Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Controllers
            app.MapControllers();

            app.Run();
        }
    }
}