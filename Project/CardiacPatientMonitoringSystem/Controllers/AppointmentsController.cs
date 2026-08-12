
using CardiacPatientMonitoringSystem.Data;
using CardiacPatientMonitoringSystem.DTOs.Requests;
using CardiacPatientMonitoringSystem.DTOs.Responses;
using CardiacPatientMonitoringSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoringSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AppointmentsController(AppDbContext context)
    {
        _context = context;
    }

    // Create a new appointment for an existing patient.
    [HttpPost]
    public async Task<IActionResult> Create(CreateAppointmentRequest request)
    {
        // Check that the patient exists before creating the appointment.
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Id == request.PatientId);

        if (!patientExists)
        {
            return NotFound("Patient not found.");
        }

        // Map the request DTO to the Appointment entity.
        var appointment = new Appointment
        {
            PatientId = request.PatientId,
            AppointmentDate = request.AppointmentDate,
            Reason = request.Reason,
            Status = request.Status
        };

        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();

        // Return a response DTO instead of exposing the entity directly.
        var response = new AppointmentResponse
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            AppointmentDate = appointment.AppointmentDate,
            Reason = appointment.Reason,
            Status = appointment.Status
        };

        return CreatedAtAction(
            nameof(GetById),
            new { id = appointment.Id },
            response
        );
    }

   
// Get all appointments with optional patient and status filters.
[HttpGet]
public async Task<IActionResult> GetAll(
    int? patientId,
    string? status)
    {
        var query = _context.Appointments.AsQueryable();

        // Filter appointments by patient when a patient ID is provided.
        if (patientId.HasValue)
        {
            query = query.Where(a => a.PatientId == patientId.Value);
        }

        // Filter appointments by status when a status is provided.
        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(a => a.Status == status);
        }

        var appointments = await query
            .Select(a => new AppointmentResponse
            {
                Id = a.Id,
                PatientId = a.PatientId,
                AppointmentDate = a.AppointmentDate,
                Reason = a.Reason,
                Status = a.Status
            })
            .ToListAsync();

        return Ok(appointments);
    }



    // Get a specific appointment by its ID.
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var appointment = await _context.Appointments
            .Where(a => a.Id == id)
            .Select(a => new AppointmentResponse
            {
                Id = a.Id,
                PatientId = a.PatientId,
                AppointmentDate = a.AppointmentDate,
                Reason = a.Reason,
                Status = a.Status
            })
            .FirstOrDefaultAsync();

        if (appointment == null)
        {
            return NotFound();
        }

        return Ok(appointment);
    }

    // Update an existing appointment.
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateAppointmentRequest request)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == id);

        if (appointment == null)
        {
            return NotFound();
        }

        // Update the editable appointment fields.
        appointment.AppointmentDate = request.AppointmentDate;
        appointment.Reason = request.Reason;
        appointment.Status = request.Status;

        await _context.SaveChangesAsync();

        return Ok(new AppointmentResponse
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            AppointmentDate = appointment.AppointmentDate,
            Reason = appointment.Reason,
            Status = appointment.Status
        });
    }

    // Delete an existing appointment.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == id);

        if (appointment == null)
        {
            return NotFound();
        }

        _context.Appointments.Remove(appointment);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
