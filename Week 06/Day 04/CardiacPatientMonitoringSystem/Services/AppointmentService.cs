using CardiacPatientMonitoringSystem.Data;
using CardiacPatientMonitoringSystem.DTOs.Requests;
using CardiacPatientMonitoringSystem.DTOs.Responses;
using CardiacPatientMonitoringSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoringSystem.Services;

public class AppointmentService : IAppointmentService
{
    private readonly AppDbContext _context;

    public AppointmentService(AppDbContext context)
    {
        _context = context;
    }

    // Patient: Get one of his own appointments.
    public async Task<AppointmentResponse?> GetByIdAsync(
        int appointmentId,
        string userId)
    {
        return await _context.Appointments
            .AsNoTracking()
            .Where(a =>
                a.Id == appointmentId &&
                a.Patient.UserId == userId)
            .Select(a => new AppointmentResponse
            {
                Id = a.Id,
                PatientId = a.PatientId,
                AppointmentDate = a.AppointmentDate,
                Reason = a.Reason,
                Status = a.Status,
                Note = a.Note != null
                    ? a.Note.Note
                    : string.Empty
            })
            .FirstOrDefaultAsync();
    }

    // Patient: Get all his own appointments.
    public async Task<IEnumerable<AppointmentResponse>> GetMyAppointmentsAsync(
        string userId)
    {
        return await _context.Appointments
            .AsNoTracking()
            .Where(a => a.Patient.UserId == userId)
            .Select(a => new AppointmentResponse
            {
                Id = a.Id,
                PatientId = a.PatientId,
                AppointmentDate = a.AppointmentDate,
                Reason = a.Reason,
                Status = a.Status,
                Note = a.Note != null
                    ? a.Note.Note
                    : string.Empty
            })
            .ToListAsync();
    }

    // Patient: Create an appointment for himself.
    // Creating an appointment and its note are handled
    // as one atomic database transaction.
    public async Task<AppointmentResponse?> CreateAsync(
        CreateAppointmentRequest request,
        string userId)
    {
        // Find the patient connected to the logged-in user.
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (patient == null)
        {
            return null;
        }

        // Prevent creating an appointment in the past.
        if (request.AppointmentDate <= DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Appointment date must be in the future.");
        }

        // Prevent the same patient from creating
        // two appointments at the same date and time.
        var appointmentExists = await _context.Appointments
            .AnyAsync(a =>
                a.PatientId == patient.Id &&
                a.AppointmentDate == request.AppointmentDate);

        if (appointmentExists)
        {
            throw new InvalidOperationException(
                "You already have an appointment at this time.");
        }

        // Start a transaction because creating an appointment
        // now involves multiple database write operations.
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            // First write operation: create the appointment.
            var appointment = new Appointment
            {
                PatientId = patient.Id,
                AppointmentDate = request.AppointmentDate,
                Reason = request.Reason,
                Status = request.Status
            };

            await _context.Appointments.AddAsync(appointment);

            // Second write operation: create the note
            // related to the appointment.
            var appointmentNote = new AppointmentNote
            {
                Appointment = appointment,
                Note = request.Note,
                CreatedAt = DateTime.UtcNow
            };

            await _context.AppointmentNotes.AddAsync(appointmentNote);

            // Save both INSERT operations together.
            await _context.SaveChangesAsync();

            // Commit only after both operations succeed.
            await transaction.CommitAsync();

            return new AppointmentResponse
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                AppointmentDate = appointment.AppointmentDate,
                Reason = appointment.Reason,
                Status = appointment.Status,
                Note = appointmentNote.Note
            };
        }
        catch
        {
            // If either operation fails, roll back all changes.
            await transaction.RollbackAsync();
            throw;
        }
    }

    // Patient: Update only his own appointment.
    public async Task<AppointmentResponse?> UpdateAsync(
        int appointmentId,
        UpdateAppointmentRequest request,
        string userId)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a =>
                a.Id == appointmentId &&
                a.Patient.UserId == userId);

        if (appointment == null)
        {
            return null;
        }

        // Prevent changing the appointment to a past date.
        if (request.AppointmentDate <= DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Appointment date must be in the future.");
        }

        // Prevent overlapping appointments.
        var appointmentExists = await _context.Appointments
            .AnyAsync(a =>
                a.Id != appointmentId &&
                a.PatientId == appointment.PatientId &&
                a.AppointmentDate == request.AppointmentDate);

        if (appointmentExists)
        {
            throw new InvalidOperationException(
                "You already have an appointment at this time.");
        }

        appointment.AppointmentDate = request.AppointmentDate;
        appointment.Reason = request.Reason;

        await _context.SaveChangesAsync();

        return new AppointmentResponse
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            AppointmentDate = appointment.AppointmentDate,
            Reason = appointment.Reason,
            Status = appointment.Status,
            Note = appointment.Note != null
                ? appointment.Note.Note
                : string.Empty
        };
    }

    // Patient: Delete only his own appointment.
    public async Task<bool> DeleteAsync(
        int appointmentId,
        string userId)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a =>
                a.Id == appointmentId &&
                a.Patient.UserId == userId);

        if (appointment == null)
        {
            return false;
        }

        _context.Appointments.Remove(appointment);

        await _context.SaveChangesAsync();

        return true;
    }
}