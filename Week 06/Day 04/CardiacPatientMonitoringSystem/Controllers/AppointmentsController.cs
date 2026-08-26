using System.Security.Claims;
using CardiacPatientMonitoringSystem.DTOs.Requests;
using CardiacPatientMonitoringSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardiacPatientMonitoringSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Patient")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(
        IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    // Patient: Create an appointment for himself.
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateAppointmentRequest request)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var appointment =
            await _appointmentService.CreateAsync(
                request,
                userId);

        if (appointment == null)
        {
            return NotFound("Patient profile not found.");
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = appointment.Id },
            appointment);
    }

    // Patient: Get all his own appointments.
    [HttpGet]
    public async Task<IActionResult> GetMyAppointments()
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var appointments =
            await _appointmentService.GetMyAppointmentsAsync(userId);

        return Ok(appointments);
    }

    // Patient: Get one of his own appointments.
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var appointment =
            await _appointmentService.GetByIdAsync(
                id,
                userId);

        if (appointment == null)
        {
            return NotFound();
        }

        return Ok(appointment);
    }

    // Patient: Update only his own appointment.
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateAppointmentRequest request)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var appointment =
            await _appointmentService.UpdateAsync(
                id,
                request,
                userId);

        if (appointment == null)
        {
            return NotFound();
        }

        return Ok(appointment);
    }

    // Patient: Delete only his own appointment.
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var deleted =
            await _appointmentService.DeleteAsync(
                id,
                userId);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}