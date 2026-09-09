namespace CardiacPatientMonitoringSystem.DTOs.Requests;

public class CreateAppointmentRequest
{
    public DateTime AppointmentDate { get; set; }

    public string Reason { get; set; } = string.Empty;

    public string Status { get; set; } = "Scheduled";

    public string Note { get; set; } = string.Empty;
}