using System.ComponentModel.DataAnnotations;

namespace CardiacPatientMonitoringSystem.DTOs.Requests;

public class CreateVitalSignRequest
{
    [Required]
    public int PatientId { get; set; }

    [Range(30, 250)]
    public int HeartRate { get; set; }

    [Range(50, 250)]
    public int SystolicBloodPressure { get; set; }

    [Range(30, 150)]
    public int DiastolicBloodPressure { get; set; }

    [Range(30, 45)]
    public decimal Temperature { get; set; }

    [Range(50, 100)]
    public int OxygenSaturation { get; set; }

    [Required]
    public DateTime RecordedAt { get; set; }
}