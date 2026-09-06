using CardiacPatientMonitoringSystem.DTOs.Auth;

namespace CardiacPatientMonitoringSystem.Services;

public interface IAuthService
{
    Task<(bool Success, string[] Errors)> RegisterAsync(
        RegisterRequest request);

    Task<string?> LoginAsync(LoginRequest request);
}