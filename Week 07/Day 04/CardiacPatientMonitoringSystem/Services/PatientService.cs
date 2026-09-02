using CardiacPatientMonitoringSystem.Data;
using CardiacPatientMonitoringSystem.DTOs;
using CardiacPatientMonitoringSystem.DTOs.Responses;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoringSystem.Services;

public class PatientService : IPatientService
{
    private readonly AppDbContext _context;

    public PatientService(AppDbContext context)
    {
        _context = context;
    }

    // Admin: Get any patient by Patient.Id
    public async Task<PatientResponse?> GetByIdAsync(int patientId)
    {
        return await _context.Patients
            .AsNoTracking()
            .Where(p => p.Id == patientId)
            .Select(p => new PatientResponse
            {
                Id = p.Id,
                FullName = p.FullName,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                PhoneNumber = p.User.PhoneNumber ?? string.Empty,
                Email = p.User.Email ?? string.Empty,
                MedicalHistory = p.MedicalHistory
            })
            .FirstOrDefaultAsync();
    }

    // Patient: Get his own profile using Identity User.Id
    public async Task<PatientResponse?> GetMyProfileAsync(string userId)
    {
        return await _context.Patients
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .Select(p => new PatientResponse
            {
                Id = p.Id,
                FullName = p.FullName,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                PhoneNumber = p.User.PhoneNumber ?? string.Empty,
                Email = p.User.Email ?? string.Empty,
                MedicalHistory = p.MedicalHistory
            })
            .FirstOrDefaultAsync();
    }

    // Admin: Get all patients with filtering, sorting and pagination
    public async Task<(IEnumerable<PatientResponse> Patients, int TotalCount)> GetAllAsync(
        int page,
        int pageSize,
        string? search,
        string? sort)
    {
        var query = _context.Patients
            .AsNoTracking()
            .AsQueryable();

        // Filtering
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                p.FullName.Contains(search) ||
                p.User.Email!.Contains(search));
        }

        // Sorting
        query = sort?.ToLower() switch
        {
            "name_desc" =>
                query.OrderByDescending(p => p.FullName),

            "dob" =>
                query.OrderBy(p => p.DateOfBirth),

            "dob_desc" =>
                query.OrderByDescending(p => p.DateOfBirth),

            _ =>
                query.OrderBy(p => p.FullName)
        };

        var totalCount = await query.CountAsync();

        var patients = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PatientResponse
            {
                Id = p.Id,
                FullName = p.FullName,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                PhoneNumber = p.User.PhoneNumber ?? string.Empty,
                Email = p.User.Email ?? string.Empty,
                MedicalHistory = p.MedicalHistory
            })
            .ToListAsync();

        return (patients, totalCount);
    }

    // Patient: Update only his own profile
    public async Task<PatientResponse?> UpdateAsync(
        int patientId,
        UpdatePatientRequest request,
        string userId)
    {
        var patient = await _context.Patients
            .Include(p => p.User)
            .FirstOrDefaultAsync(p =>
                p.Id == patientId &&
                p.UserId == userId);

        if (patient == null)
        {
            return null;
        }

        // Update Patient data
        patient.FullName = request.FullName;
        patient.DateOfBirth = request.DateOfBirth;
        patient.Gender = request.Gender;
        patient.MedicalHistory = request.MedicalHistory;

        // Update Identity data
        patient.User.PhoneNumber = request.PhoneNumber;
        patient.User.Email = request.Email;
        patient.User.UserName = request.Email;

        await _context.SaveChangesAsync();

        return new PatientResponse
        {
            Id = patient.Id,
            FullName = patient.FullName,
            DateOfBirth = patient.DateOfBirth,
            Gender = patient.Gender,
            PhoneNumber = patient.User.PhoneNumber ?? string.Empty,
            Email = patient.User.Email ?? string.Empty,
            MedicalHistory = patient.MedicalHistory
        };
    }

    // Admin: Delete patient
    public async Task<bool> DeleteAsync(int patientId)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.Id == patientId);

        if (patient == null)
        {
            return false;
        }

        _context.Patients.Remove(patient);

        await _context.SaveChangesAsync();

        return true;
    }
}