using CardiacPatientMonitoringSystem.Models;

namespace CardiacPatientMonitoringSystem.DTOs.Requests;

public class CreatePatientRequest
{
    public string FullName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public Gender Gender { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string MedicalHistory { get; set; } = string.Empty;
}