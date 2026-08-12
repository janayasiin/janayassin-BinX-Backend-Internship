
using System.ComponentModel.DataAnnotations;

namespace CardiacPatientMonitoringSystem.DTOs.Requests;

public class UpdateMedicationRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Dosage { get; set; } = string.Empty;

    [Required]
    public string Frequency { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}

