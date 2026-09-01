using CardiacPatientMonitoringSystem.Data;
using CardiacPatientMonitoringSystem.DTOs.Requests;
using CardiacPatientMonitoringSystem.DTOs.Responses;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoringSystem.Services;

public class VitalSignRecordService : IVitalSignRecordService
{
    private readonly AppDbContext _context;
    private readonly IVitalSignAnalysisService _analysisService;
    private readonly IVitalSignEmailService _vitalSignEmailService;
    public VitalSignRecordService(
     AppDbContext context,
     IVitalSignAnalysisService analysisService,
     IVitalSignEmailService vitalSignEmailService)
    {
        _context = context;
        _analysisService = analysisService;
        _vitalSignEmailService = vitalSignEmailService;
    }

    // Patient: Get all his own vital signs.

    public async Task<IEnumerable<VitalSignResponse>> GetMyVitalSignsAsync(
    string userId)
    {
        var vitalSigns = await _context.VitalSigns
            .AsNoTracking()
            .Where(v => v.Patient.UserId == userId)
            .OrderByDescending(v => v.RecordedAt)
            .ToListAsync();

        var responses = new List<VitalSignResponse>();

        foreach (var vitalSign in vitalSigns)
        {
            var analysis = _analysisService.Analyze(vitalSign);

            responses.Add(new VitalSignResponse
            {
                Id = vitalSign.Id,
                PatientId = vitalSign.PatientId,
                HeartRate = vitalSign.HeartRate,
                SystolicBloodPressure = vitalSign.SystolicBloodPressure,
                DiastolicBloodPressure = vitalSign.DiastolicBloodPressure,
                Temperature = vitalSign.Temperature,
                OxygenSaturation = vitalSign.OxygenSaturation,
                RecordedAt = vitalSign.RecordedAt,
                Status = analysis.Status,
                Alerts = analysis.Alerts
            });
        }

        return responses;
    }


    // Patient: Get one of his own vital signs.
    public async Task<VitalSignResponse?> GetByIdAsync(
      int vitalSignId,
      string userId)
    {
        var vitalSign = await _context.VitalSigns
            .AsNoTracking()
            .FirstOrDefaultAsync(v =>
                v.Id == vitalSignId &&
                v.Patient.UserId == userId);

        if (vitalSign == null)
        {
            return null;
        }

        var analysis = _analysisService.Analyze(vitalSign);

        return new VitalSignResponse
        {
            Id = vitalSign.Id,
            PatientId = vitalSign.PatientId,
            HeartRate = vitalSign.HeartRate,
            SystolicBloodPressure = vitalSign.SystolicBloodPressure,
            DiastolicBloodPressure = vitalSign.DiastolicBloodPressure,
            Temperature = vitalSign.Temperature,
            OxygenSaturation = vitalSign.OxygenSaturation,
            RecordedAt = vitalSign.RecordedAt,

            Status = analysis.Status,
            Alerts = analysis.Alerts
        };
    }

    // Patient: Create a vital-sign record for himself.
    public async Task<VitalSignResponse?> CreateAsync(
        CreateVitalSignRequest request,
        string userId)
    {
        // Find the patient connected to the logged-in user.
        var patient = await _context.Patients
     .Include(p => p.User)
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
        var analysis = _analysisService.Analyze(vitalSign);
        if (analysis.Status == "Critical")
        {
            await _vitalSignEmailService.SendCriticalAlertAsync(
                patient.User.Email!,
                analysis);
        }
        return new VitalSignResponse
        {
            Id = vitalSign.Id,
            PatientId = vitalSign.PatientId,
            HeartRate = vitalSign.HeartRate,
            SystolicBloodPressure = vitalSign.SystolicBloodPressure,
            DiastolicBloodPressure = vitalSign.DiastolicBloodPressure,
            Temperature = vitalSign.Temperature,
            OxygenSaturation = vitalSign.OxygenSaturation,
            RecordedAt = vitalSign.RecordedAt,

            Status = analysis.Status,
            Alerts = analysis.Alerts
        };
    }


    // Patient: Update only his own vital-sign record.

    public async Task<VitalSignResponse?> UpdateAsync(
        int vitalSignId,
        UpdateVitalSignRequest request,
        string userId)
    {
        var vitalSign = await _context.VitalSigns
            .Include(v => v.Patient)
            .ThenInclude(p => p.User)
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

        // Analyze the old values before updating
        var oldAnalysis = _analysisService.Analyze(vitalSign);

        // Update the vital signs
        vitalSign.HeartRate = request.HeartRate;

        vitalSign.SystolicBloodPressure =
            request.SystolicBloodPressure;

        vitalSign.DiastolicBloodPressure =
            request.DiastolicBloodPressure;

        vitalSign.Temperature =
            request.Temperature;

        vitalSign.OxygenSaturation =
            request.OxygenSaturation;

        vitalSign.RecordedAt =
            request.RecordedAt;

        await _context.SaveChangesAsync();

        // Analyze the new values
        var newAnalysis = _analysisService.Analyze(vitalSign);

        // Send email only when status changes to Critical
        if (oldAnalysis.Status != "Critical" &&
     newAnalysis.Status == "Critical")
        {
            await _vitalSignEmailService.SendCriticalAlertAsync(
                vitalSign.Patient.User.Email!,
                newAnalysis,
                true);
        }

        return new VitalSignResponse
        {
            Id = vitalSign.Id,
            PatientId = vitalSign.PatientId,
            HeartRate = vitalSign.HeartRate,
            SystolicBloodPressure =
                vitalSign.SystolicBloodPressure,
            DiastolicBloodPressure =
                vitalSign.DiastolicBloodPressure,
            Temperature =
                vitalSign.Temperature,
            OxygenSaturation =
                vitalSign.OxygenSaturation,
            RecordedAt =
                vitalSign.RecordedAt,

            Status = newAnalysis.Status,
            Alerts = newAnalysis.Alerts
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