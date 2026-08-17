namespace CardiacPatientMonitoringSystem.Services;

public interface IPatientRepository
{
    Task<int> GetPatientAgeAsync(int patientId);
}