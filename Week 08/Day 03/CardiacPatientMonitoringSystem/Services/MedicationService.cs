using CardiacPatientMonitoringSystem.Data;
using CardiacPatientMonitoringSystem.DTOs.Requests;
using CardiacPatientMonitoringSystem.DTOs.Responses;
using CardiacPatientMonitoringSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoringSystem.Services;

public class MedicationService : IMedicationService
{
    private readonly AppDbContext _context;

    public MedicationService(AppDbContext context)
    {
        _context = context;
    }

    // Patient: Get all his own medications.
    public async Task<IEnumerable<MedicationResponse>> GetMyMedicationsAsync(
        string userId)
    {
        return await _context.Medications
            .AsNoTracking()
            .Where(m => m.Patient.UserId == userId)
            .Select(m => new MedicationResponse
            {
                Id = m.Id,
                PatientId = m.PatientId,
                Name = m.Name,
                Dosage = m.Dosage,
                Frequency = m.Frequency,
                StartDate = m.StartDate,
                EndDate = m.EndDate
            })
            .ToListAsync();
    }

    // Patient: Get one of his own medications.
    public async Task<MedicationResponse?> GetByIdAsync(
        int medicationId,
        string userId)
    {
        return await _context.Medications
            .AsNoTracking()
            .Where(m =>
                m.Id == medicationId &&
                m.Patient.UserId == userId)
            .Select(m => new MedicationResponse
            {
                Id = m.Id,
                PatientId = m.PatientId,
                Name = m.Name,
                Dosage = m.Dosage,
                Frequency = m.Frequency,
                StartDate = m.StartDate,
                EndDate = m.EndDate
            })
            .FirstOrDefaultAsync();
    }

    // Patient: Create a medication for himself.
    public async Task<MedicationResponse?> CreateAsync(
        CreateMedicationRequest request,
        string userId)
    {
        // Find the patient connected to the logged-in user.
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (patient == null)
        {
            return null;
        }

        // Prevent an invalid date range.
        if (request.EndDate.HasValue &&
            request.EndDate.Value <= request.StartDate)
        {
            throw new ArgumentException(
                "End date must be after the start date.");
        }

        var medication = new Medication
        {
            PatientId = patient.Id,
            Name = request.Name,
            Dosage = request.Dosage,
            Frequency = request.Frequency,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        await _context.Medications.AddAsync(medication);

        await _context.SaveChangesAsync();

        return new MedicationResponse
        {
            Id = medication.Id,
            PatientId = medication.PatientId,
            Name = medication.Name,
            Dosage = medication.Dosage,
            Frequency = medication.Frequency,
            StartDate = medication.StartDate,
            EndDate = medication.EndDate
        };
    }

    // Patient: Update only his own medication.
    public async Task<MedicationResponse?> UpdateAsync(
        int medicationId,
        UpdateMedicationRequest request,
        string userId)
    {
        var medication = await _context.Medications
            .FirstOrDefaultAsync(m =>
                m.Id == medicationId &&
                m.Patient.UserId == userId);

        if (medication == null)
        {
            return null;
        }

        // Prevent an invalid date range.
        if (request.EndDate.HasValue &&
            request.EndDate.Value <= request.StartDate)
        {
            throw new ArgumentException(
                "End date must be after the start date.");
        }

        medication.Name = request.Name;
        medication.Dosage = request.Dosage;
        medication.Frequency = request.Frequency;
        medication.StartDate = request.StartDate;
        medication.EndDate = request.EndDate;

        await _context.SaveChangesAsync();

        return new MedicationResponse
        {
            Id = medication.Id,
            PatientId = medication.PatientId,
            Name = medication.Name,
            Dosage = medication.Dosage,
            Frequency = medication.Frequency,
            StartDate = medication.StartDate,
            EndDate = medication.EndDate
        };
    }

    // Patient: Delete only his own medication.
    public async Task<bool> DeleteAsync(
        int medicationId,
        string userId)
    {
        var medication = await _context.Medications
            .FirstOrDefaultAsync(m =>
                m.Id == medicationId &&
                m.Patient.UserId == userId);

        if (medication == null)
        {
            return false;
        }

        _context.Medications.Remove(medication);

        await _context.SaveChangesAsync();

        return true;
    }
}