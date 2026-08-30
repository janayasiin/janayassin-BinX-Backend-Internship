namespace CardiacPatientMonitoringSystem.DTOs.Requests;

public class UpdateAppointmentRequest
{
    public DateTime AppointmentDate { get; set; }

    public string Reason { get; set; } = string.Empty;
}