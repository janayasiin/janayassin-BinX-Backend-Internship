using CardiacPatientMonitoringSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoringSystem.Data;

public static class SeedData
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        if (await context.Patients.AnyAsync())
        {
            return;
        }

        var patients = new List<Patient>
        {
            new Patient
            {
                FullName = "Ahmad Ali",
                DateOfBirth = new DateTime(1985, 5, 12),
                Gender = Gender.Male,
                PhoneNumber = "0590000001",
                Email = "ahmad@example.com",
                MedicalHistory = "Hypertension"
            },
            new Patient
            {
                FullName = "Sara Hassan",
                DateOfBirth = new DateTime(1990, 8, 20),
                Gender = Gender.Female,
                PhoneNumber = "0590000002",
                Email = "sara@example.com",
                MedicalHistory = "Previous cardiac follow-up"
            },
            new Patient
            {
                FullName = "Omar Khaled",
                DateOfBirth = new DateTime(1978, 2, 3),
                Gender = Gender.Male,
                PhoneNumber = "0590000003",
                Email = "omar@example.com",
                MedicalHistory = "High cholesterol"
            }
        };

        await context.Patients.AddRangeAsync(patients);
        await context.SaveChangesAsync();

        var vitalSigns = new List<VitalSign>
        {
            new VitalSign
            {
                PatientId = patients[0].Id,
                HeartRate = 78,
                SystolicBloodPressure = 120,
                DiastolicBloodPressure = 80,
                Temperature = 36.7m,
                OxygenSaturation = 98,
                RecordedAt = DateTime.UtcNow
            },
            new VitalSign
            {
                PatientId = patients[1].Id,
                HeartRate = 82,
                SystolicBloodPressure = 125,
                DiastolicBloodPressure = 82,
                Temperature = 36.8m,
                OxygenSaturation = 97,
                RecordedAt = DateTime.UtcNow
            },
            new VitalSign
            {
                PatientId = patients[2].Id,
                HeartRate = 75,
                SystolicBloodPressure = 118,
                DiastolicBloodPressure = 78,
                Temperature = 36.6m,
                OxygenSaturation = 99,
                RecordedAt = DateTime.UtcNow
            }
        };

        await context.VitalSigns.AddRangeAsync(vitalSigns);

        var medications = new List<Medication>
        {
            new Medication
            {
                PatientId = patients[0].Id,
                Name = "Aspirin",
                Dosage = "100 mg",
                Frequency = "Once daily",
                StartDate = new DateTime(2026, 8, 1)
            },
            new Medication
            {
                PatientId = patients[1].Id,
                Name = "Atorvastatin",
                Dosage = "20 mg",
                Frequency = "Once daily",
                StartDate = new DateTime(2026, 8, 1)
            },
            new Medication
            {
                PatientId = patients[2].Id,
                Name = "Bisoprolol",
                Dosage = "5 mg",
                Frequency = "Once daily",
                StartDate = new DateTime(2026, 8, 1)
            }
        };

        await context.Medications.AddRangeAsync(medications);

        var appointments = new List<Appointment>
        {
            new Appointment
            {
                PatientId = patients[0].Id,
                AppointmentDate = new DateTime(2026, 8, 15, 10, 0, 0),
                Reason = "Cardiac follow-up",
                Status = "Scheduled"
            },
            new Appointment
            {
                PatientId = patients[1].Id,
                AppointmentDate = new DateTime(2026, 8, 17, 11, 30, 0),
                Reason = "Routine check-up",
                Status = "Scheduled"
            },
            new Appointment
            {
                PatientId = patients[2].Id,
                AppointmentDate = new DateTime(2026, 8, 20, 9, 0, 0),
                Reason = "Blood pressure follow-up",
                Status = "Scheduled"
            }
        };

        await context.Appointments.AddRangeAsync(appointments);

        await context.SaveChangesAsync();
    }
}