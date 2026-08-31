using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CardiacPatientMonitoringSystem.Data;
using CardiacPatientMonitoringSystem.DTOs.Auth;
using CardiacPatientMonitoringSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CardiacPatientMonitoringSystem.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _context = context;
    }

    public async Task<(bool Success, string[] Errors)> RegisterAsync(
    RegisterRequest request)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            // Create Identity user
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(
                user,
                request.Password);

            if (!result.Succeeded)
            {
                await transaction.RollbackAsync();

                return (
                    false,
                    result.Errors
                        .Select(e => e.Description)
                        .ToArray()
                );
            }

            // Create Patient profile linked to the Identity user
            var patient = new Patient
            {
                UserId = user.Id,
                FullName = request.FullName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                MedicalHistory = request.MedicalHistory
            };

            await _context.Patients.AddAsync(patient);

            // Assign Patient role
            var roleResult = await _userManager.AddToRoleAsync(
                user,
                "Patient");

            if (!roleResult.Succeeded)
            {
                await transaction.RollbackAsync();

                return (
                    false,
                    roleResult.Errors
                        .Select(e => e.Description)
                        .ToArray()
                );
            }

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return (true, Array.Empty<string>());
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public async Task<string?> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(
            request.Email);

        if (user == null)
        {
            return null;
        }

        var result = await _signInManager.CheckPasswordSignInAsync(
            user,
            request.Password,
            false);

        if (!result.Succeeded)
        {
            return null;
        }
        var patient = await _context.Patients
    .FirstOrDefaultAsync(p => p.UserId == user.Id);

        if (patient == null)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(
                JwtRegisteredClaimNames.Sub,
                user.Id),

            new Claim(
                ClaimTypes.NameIdentifier,
                user.Id),

            new Claim(
                ClaimTypes.Email,
                user.Email!),
            new Claim(
    "PatientId",
    patient.Id.ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(
                new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}