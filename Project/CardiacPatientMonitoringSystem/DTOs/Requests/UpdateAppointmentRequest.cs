
using System.ComponentModel.DataAnnotations;

namespace CardiacPatientMonitoringSystem.DTOs.Requests;

public class UpdateAppointmentRequest
{

    [Required]
    public DateTime AppointmentDate { get; set; }

    [Required]
    public string Reason { get; set; } = string.Empty;

    [Required]
    public string Status { get; set; } = "Scheduled";
}

