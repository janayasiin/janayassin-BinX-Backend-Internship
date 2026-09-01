using Microsoft.AspNetCore.Identity;

namespace CardiacPatientMonitoringSystem.Models;

public class ApplicationUser : IdentityUser
{
    public Patient? Patient { get; set; }
}