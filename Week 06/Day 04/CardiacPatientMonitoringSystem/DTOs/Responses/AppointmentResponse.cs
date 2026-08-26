namespace CardiacPatientMonitoringSystem.DTOs.Responses;

public class AppointmentResponse
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string Reason { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string Note { get; set; } = string.Empty;
}