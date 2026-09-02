namespace CardiacPatientMonitoringSystem.DTOs.Responses
{

    public class VitalSignAnalysisResponse
    {
        public string Status { get; set; } = string.Empty;

        public List<string> Alerts { get; set; } = new();
    }
}
