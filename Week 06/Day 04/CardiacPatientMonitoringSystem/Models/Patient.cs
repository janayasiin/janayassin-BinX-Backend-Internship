namespace CardiacPatientMonitoringSystem.Models;

public enum Gender
{
    Male,
    Female
}

public class Patient
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public Gender Gender { get; set; }

    public string MedicalHistory { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;

    public ICollection<VitalSign> VitalSigns { get; set; } = new List<VitalSign>();

    public ICollection<Medication> Medications { get; set; } = new List<Medication>();

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}