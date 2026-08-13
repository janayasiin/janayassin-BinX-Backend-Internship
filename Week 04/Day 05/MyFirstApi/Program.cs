using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
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

            // Read the allowed frontend origins from appsettings.json.
            // This keeps CORS configuration outside the application code.
            var allowedOrigins =
                builder.Configuration
                    .GetSection("AllowedOrigins")
                    .Get<string[]>()
                ?? Array.Empty<string>();

            // Configure CORS to allow only the known frontend origins.
            // This is safer than allowing requests from any website.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy
                        .WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // Configure rate limiting to control how many requests
            // a client can make within a specific time window.
            builder.Services.AddRateLimiter(options =>
            {
                // Login gets a stricter limit because it is a sensitive endpoint
                // that can be targeted by brute-force password attempts.
                options.AddFixedWindowLimiter("LoginPolicy", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 5;
                    limiterOptions.Window = TimeSpan.FromMinutes(1);
                    limiterOptions.QueueLimit = 0;
                });

                // General API endpoints have a higher request limit.
                options.AddFixedWindowLimiter("GeneralPolicy", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 100;
                    limiterOptions.Window = TimeSpan.FromMinutes(1);
                    limiterOptions.QueueLimit = 0;
                });

                // Return HTTP 429 when the client exceeds its rate limit.
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });

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

            // HSTS tells browsers to use HTTPS for this application.
            // It is enabled only outside Development.
            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI();

            // Custom request logging middleware.
            app.UseMiddleware<RequestLoggingMiddleware>();

            // Redirect HTTP requests to HTTPS.
            app.UseHttpsRedirection();

            // Content Security Policy limits the sources that a browser
            // is allowed to load for this application.
            app.Use(async (context, next) =>
            {
                context.Response.Headers["Content-Security-Policy"] =
                    "default-src 'self'";

                await next();
            });
            // Apply the configured rate-limiting policies.
            app.UseRateLimiter();

            // Apply the named CORS policy to incoming requests.
            app.UseCors("AllowFrontend");

            // Authentication must come before Authorization.
            app.UseAuthentication();
            app.UseAuthorization();

            // Map controller endpoints.
            app.MapControllers();

            app.Run();
        }
    }
}