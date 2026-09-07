using CardiacPatientMonitoringSystem.DTOs.Requests;
using CardiacPatientMonitoringSystem.DTOs.Responses;

namespace CardiacPatientMonitoringSystem.Services;

public interface IMedicationService
{
    Task<IEnumerable<MedicationResponse>> GetMyMedicationsAsync(
        string userId);

    Task<MedicationResponse?> GetByIdAsync(
        int medicationId,
        string userId);

    Task<MedicationResponse?> CreateAsync(
        CreateMedicationRequest request,
        string userId);

    Task<MedicationResponse?> UpdateAsync(
        int medicationId,
        UpdateMedicationRequest request,
        string userId);

    Task<bool> DeleteAsync(
        int medicationId,
        string userId);
}