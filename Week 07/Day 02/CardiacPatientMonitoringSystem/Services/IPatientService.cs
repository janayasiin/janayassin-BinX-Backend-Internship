using CardiacPatientMonitoringSystem.DTOs;
using CardiacPatientMonitoringSystem.DTOs.Responses;

namespace CardiacPatientMonitoringSystem.Services;

public interface IPatientService
{
    Task<PatientResponse?> GetByIdAsync(int patientId);

    Task<PatientResponse?> GetMyProfileAsync(string userId);

    Task<(IEnumerable<PatientResponse> Patients, int TotalCount)> GetAllAsync(
        int page,
        int pageSize,
        string? search,
        string? sort);

    Task<PatientResponse?> UpdateAsync(
        int patientId,
        UpdatePatientRequest request,
        string userId);

    Task<bool> DeleteAsync(int patientId);
}       