namespace CardiacPatientMonitoringSystem.DTOs.Responses;

public class VitalSignResponse
{
    public int Id { get; set; }
    public int PatientId { get; set; }

    public int HeartRate { get; set; }
    public int SystolicBloodPressure { get; set; }
    public int DiastolicBloodPressure { get; set; }
    public decimal Temperature { get; set; }
    public int OxygenSaturation { get; set; }

    public DateTime RecordedAt { get; set; }

    public string Status { get; set; } = string.Empty;

    public List<string> Alerts { get; set; } = new();
} 