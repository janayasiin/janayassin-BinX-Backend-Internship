using CardiacPatientMonitoringSystem.Data;
using CardiacPatientMonitoringSystem.Middleware;
using CardiacPatientMonitoringSystem.Models;
using CardiacPatientMonitoringSystem.Services;
using CardiacPatientMonitoringSystem.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CardiacPatientMonitoringSystem;


public static class PerformanceTestDataSeeder
{
    public static async Task InitializeAsync(
        UserManager<ApplicationUser> userManager,
        AppDbContext context)
    {
        // Check if performance test data already exists.
        if (await context.Patients.CountAsync() >= 50)
        {
            return;
        }

        for (int i = 1; i <= 50; i++)
        {
            var email = $"performance.patient{i}@test.com";

            var existingUser = await userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                continue;
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                PhoneNumber = $"059000{i:D4}"
            };

            var result = await userManager.CreateAsync(
                user,
                "Test1234!");

            if (!result.Succeeded)
            {
                continue;
            }

            var patient = new Patient
            {
                UserId = user.Id,
                FullName = $"Performance Patient {i}",
                DateOfBirth = new DateTime(1990, 1, 1).AddDays(i),
                Gender = i % 2 == 0
                    ? Gender.Female
                    : Gender.Male,
                MedicalHistory = "Performance testing data"
            };

            context.Patients.Add(patient);

            await context.SaveChangesAsync();

            var medication = new Medication
            {
                PatientId = patient.Id,
                Name = "Test Medication",
                Dosage = "10 mg",
                Frequency = "Once daily",
                StartDate = DateTime.UtcNow.AddDays(-30),
                EndDate = null
            };

            context.Medications.Add(medication);

            await context.SaveChangesAsync();
        }
    }
}
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Controllers
        builder.Services.AddControllers();

        // Services
        builder.Services.AddScoped<IPatientService, PatientService>();
        builder.Services.AddScoped<IVitalSignRecordService, VitalSignRecordService>();

        builder.Services.AddScoped<IMedicationService, MedicationService>();
        builder.Services.AddScoped<IAppointmentService, AppointmentService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IVitalSignAnalysisService, VitalSignAnalysisService>();
        builder.Services.AddScoped<IEmailService, SmtpEmailService>();
        builder.Services.AddScoped<IVitalSignEmailService, VitalSignEmailService>();
        // FluentValidation
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<
            UpdatePatientRequestValidator>();

        // Swagger
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(
                "Bearer",
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter your JWT token. Example: Bearer {token}"
                });

            options.AddSecurityRequirement(
                new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference =
                        new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                },
                Array.Empty<string>()
            }
                });
        });

      
      
// Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (builder.Environment.IsEnvironment("Testing"))
    {
        options.UseInMemoryDatabase(
            "CardiacPatientMonitoringTestDb");
    }
    else
    {
        options.UseSqlServer(
            builder.Configuration.GetConnectionString(
                "DefaultConnection"));

        if (builder.Environment.IsDevelopment())
        {
            options
                .LogTo(
                    Console.WriteLine,
                    LogLevel.Information)
                .EnableSensitiveDataLogging();
        }
    }
});



        // ASP.NET Core Identity
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

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
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer =
                            builder.Configuration["Jwt:Issuer"],

                        ValidAudience =
                            builder.Configuration["Jwt:Audience"],

                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(
                                    builder.Configuration["Jwt:Key"]!
                                )
                            )
                    };
            });

        // Authorization
        builder.Services.AddAuthorization();

        var app = builder.Build();

        // Seed roles and default admin
        using (var scope = app.Services.CreateScope())
        {
            var roleManager =
                scope.ServiceProvider
                    .GetRequiredService<RoleManager<IdentityRole>>();

            var userManager =
                scope.ServiceProvider
                    .GetRequiredService<UserManager<ApplicationUser>>();

            await RoleSeedData.InitializeAsync(roleManager);

            await AdminSeedData.InitializeAsync(
                userManager,
                builder.Configuration);

            await PerformanceTestDataSeeder.InitializeAsync(
                userManager,
                scope.ServiceProvider.GetRequiredService<AppDbContext>());
        }

        // Custom middleware
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseMiddleware<RequestTimingMiddleware>();
        // Swagger
        app.UseSwagger();
        app.UseSwaggerUI();

        // HTTPS
        app.UseHttpsRedirection();

        // Authentication must come before Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // Controllers
        app.MapControllers();

        app.Run();
    }
}