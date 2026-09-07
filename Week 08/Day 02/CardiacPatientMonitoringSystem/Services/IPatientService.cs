using CardiacPatientMonitoringSystem.DTOs;
using CardiacPatientMonitoringSystem.DTOs.Responses;

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
    Task<int> GetPatientsWithMultipleCollectionsAsync();
    Task<bool> DeleteAsync(int patientId);

    Task<int> GetPatientsWithNPlusOneAsync();
}