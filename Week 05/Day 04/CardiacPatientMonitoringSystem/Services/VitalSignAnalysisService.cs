namespace CardiacPatientMonitoringSystem.Services;

public class VitalSignAnalysisService
{
    private readonly IPatientRepository _patientRepository;

    public VitalSignAnalysisService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<bool> IsPatientAdultAsync(int patientId)
    {
        try
        {
            var age = await _patientRepository.GetPatientAgeAsync(patientId);

            return age >= 18;
        }
        catch (Exception)
        {
            return false;
        }
    }
}