namespace CardiacPatientMonitoringSystem.Models;

public class AppointmentNote
{
    public int Id { get; set; }

    public int AppointmentId { get; set; }

    public string Note { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public Appointment Appointment { get; set; } = null!;
}