using CardiacPatientMonitoringSystem.DTOs.Responses;
using CardiacPatientMonitoringSystem.Models;

namespace CardiacPatientMonitoringSystem.Services
{
    public interface IVitalSignAnalysisService
    {
        VitalSignAnalysisResponse Analyze(
            VitalSign vitalSign);
    }
}