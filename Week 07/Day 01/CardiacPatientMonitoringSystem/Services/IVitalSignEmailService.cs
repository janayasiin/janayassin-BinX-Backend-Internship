using CardiacPatientMonitoringSystem.DTOs.Responses;

namespace CardiacPatientMonitoringSystem.Services;

public interface IVitalSignEmailService
{
    Task SendCriticalAlertAsync(
        string email,
        VitalSignAnalysisResponse analysis,
        bool isUpdate = false);
}