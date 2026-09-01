using CardiacPatientMonitoringSystem.DTOs.Requests;
using CardiacPatientMonitoringSystem.DTOs.Responses;

namespace CardiacPatientMonitoringSystem.Services;

public interface IVitalSignRecordService
{
    Task<IEnumerable<VitalSignResponse>> GetMyVitalSignsAsync(
        string userId);

    Task<VitalSignResponse?> GetByIdAsync(
        int vitalSignId,
        string userId);

    Task<VitalSignResponse?> CreateAsync(
        CreateVitalSignRequest request,
        string userId);

    Task<VitalSignResponse?> UpdateAsync(
        int vitalSignId,
        UpdateVitalSignRequest request,
        string userId);

    Task<bool> DeleteAsync(
        int vitalSignId,
        string userId);
}