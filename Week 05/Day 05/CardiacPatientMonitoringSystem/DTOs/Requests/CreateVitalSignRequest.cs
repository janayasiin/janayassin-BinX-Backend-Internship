using System.ComponentModel.DataAnnotations;

namespace CardiacPatientMonitoringSystem.DTOs.Requests;

public class CreateVitalSignRequest
{
    public int PatientId { get; set; }

    public int HeartRate { get; set; }

    public int SystolicBloodPressure { get; set; }

    
    public int DiastolicBloodPressure { get; set; }


    public decimal Temperature { get; set; }

    public int OxygenSaturation { get; set; }


    public DateTime RecordedAt { get; set; }
}