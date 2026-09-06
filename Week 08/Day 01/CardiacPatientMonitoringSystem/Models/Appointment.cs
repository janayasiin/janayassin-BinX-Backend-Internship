namespace CardiacPatientMonitoringSystem.Models;

public class Appointment
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string Reason { get; set; } = string.Empty;

    public string Status { get; set; } = "Scheduled";

    public Patient Patient { get; set; } = null!;

    public AppointmentNote? Note { get; set; }
}