using System.ComponentModel.DataAnnotations;
using CardiacPatientMonitoringSystem.Models;

namespace CardiacPatientMonitoringSystem.DTOs;

public class UpdatePatientRequest
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string MedicalHistory { get; set; } = string.Empty;
}