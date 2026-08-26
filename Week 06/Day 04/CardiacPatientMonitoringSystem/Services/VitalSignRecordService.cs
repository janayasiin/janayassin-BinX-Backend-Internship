using CardiacPatientMonitoringSystem.Data;
using CardiacPatientMonitoringSystem.DTOs.Requests;
using CardiacPatientMonitoringSystem.DTOs.Responses;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoringSystem.Services;

public class VitalSignRecordService : IVitalSignRecordService
{
    private readonly AppDbContext _context;

    public VitalSignRecordService(AppDbContext context)
    {
        _context = context;
    }

    // Patient: Get all his own vital signs.
    public async Task<IEnumerable<VitalSignResponse>> GetMyVitalSignsAsync(
        string userId)
    {
        return await _context.VitalSigns
            .AsNoTracking()
            .Where(v => v.Patient.UserId == userId)
            .OrderByDescending(v => v.RecordedAt)
            .Select(v => new VitalSignResponse
            {
                Id = v.Id,
                PatientId = v.PatientId,
                HeartRate = v.HeartRate,
                SystolicBloodPressure = v.SystolicBloodPressure,
                DiastolicBloodPressure = v.DiastolicBloodPressure,
                Temperature = v.Temperature,
                OxygenSaturation = v.OxygenSaturation,
                RecordedAt = v.RecordedAt
            })
            .ToListAsync();
    }

    // Patient: Get one of his own vital signs.
    public async Task<VitalSignResponse?> GetByIdAsync(
        int vitalSignId,
        string userId)
    {
        return await _context.VitalSigns
            .AsNoTracking()
            .Where(v =>
                v.Id == vitalSignId &&
                v.Patient.UserId == userId)
            .Select(v => new VitalSignResponse
            {
                Id = v.Id,
                PatientId = v.PatientId,
                HeartRate = v.HeartRate,
                SystolicBloodPressure = v.SystolicBloodPressure,
                DiastolicBloodPressure = v.DiastolicBloodPressure,
                Temperature = v.Temperature,
                OxygenSaturation = v.OxygenSaturation,
                RecordedAt = v.RecordedAt
            })
            .FirstOrDefaultAsync();
    }

    // Patient: Create a vital-sign record for himself.
    public async Task<VitalSignResponse?> CreateAsync(
        CreateVitalSignRequest request,
        string userId)
    {
        // Find the patient connected to the logged-in user.
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (patient == null)
        {
            return null;
        }

        // Prevent recording a reading in the future.
        if (request.RecordedAt > DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Recorded date cannot be in the future.");
        }

        var vitalSign = new Models.VitalSign
        {
            PatientId = patient.Id,
            HeartRate = request.HeartRate,
            SystolicBloodPressure =
                request.SystolicBloodPressure,
            DiastolicBloodPressure =
                request.DiastolicBloodPressure,
            Temperature = request.Temperature,
            OxygenSaturation = request.OxygenSaturation,
            RecordedAt = request.RecordedAt
        };

        await _context.VitalSigns.AddAsync(vitalSign);
        await _context.SaveChangesAsync();

        return new VitalSignResponse
        {
            Id = vitalSign.Id,
            PatientId = vitalSign.PatientId,
            HeartRate = vitalSign.HeartRate,
            SystolicBloodPressure =
                vitalSign.SystolicBloodPressure,
            DiastolicBloodPressure =
                vitalSign.DiastolicBloodPressure,
            Temperature = vitalSign.Temperature,
            OxygenSaturation =
                vitalSign.OxygenSaturation,
            RecordedAt = vitalSign.RecordedAt
        };
    }

    // Patient: Update only his own vital-sign record.
    public async Task<VitalSignResponse?> UpdateAsync(
        int vitalSignId,
        UpdateVitalSignRequest request,
        string userId)
    {
        var vitalSign = await _context.VitalSigns
            .FirstOrDefaultAsync(v =>
                v.Id == vitalSignId &&
                v.Patient.UserId == userId);

        if (vitalSign == null)
        {
            return null;
        }

        if (request.RecordedAt > DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Recorded date cannot be in the future.");
        }

        vitalSign.HeartRate = request.HeartRate;
        vitalSign.SystolicBloodPressure =
            request.SystolicBloodPressure;
        vitalSign.DiastolicBloodPressure =
            request.DiastolicBloodPressure;
        vitalSign.Temperature = request.Temperature;
        vitalSign.OxygenSaturation =
            request.OxygenSaturation;
        vitalSign.RecordedAt = request.RecordedAt;

        await _context.SaveChangesAsync();

        return new VitalSignResponse
        {
            Id = vitalSign.Id,
            PatientId = vitalSign.PatientId,
            HeartRate = vitalSign.HeartRate,
            SystolicBloodPressure =
                vitalSign.SystolicBloodPressure,
            DiastolicBloodPressure =
                vitalSign.DiastolicBloodPressure,
            Temperature = vitalSign.Temperature,
            OxygenSaturation =
                vitalSign.OxygenSaturation,
            RecordedAt = vitalSign.RecordedAt
        };
    }

    // Patient: Delete only his own vital-sign record.
    public async Task<bool> DeleteAsync(
        int vitalSignId,
        string userId)
    {
        var vitalSign = await _context.VitalSigns
            .FirstOrDefaultAsync(v =>
                v.Id == vitalSignId &&
                v.Patient.UserId == userId);

        if (vitalSign == null)
        {
            return false;
        }

        _context.VitalSigns.Remove(vitalSign);

        await _context.SaveChangesAsync();

        return true;
    }
}