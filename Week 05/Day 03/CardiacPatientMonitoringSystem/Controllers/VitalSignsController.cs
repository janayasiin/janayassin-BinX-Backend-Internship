
using CardiacPatientMonitoringSystem.Data;
using CardiacPatientMonitoringSystem.DTOs.Requests;
using CardiacPatientMonitoringSystem.DTOs.Responses;
using CardiacPatientMonitoringSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoringSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VitalSignsController : ControllerBase
{
    private readonly AppDbContext _context;

    public VitalSignsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateVitalSignRequest request)
    {
        // Check that the patient exists before creating a vital sign.
        // This prevents creating a record linked to a non-existing patient.
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Id == request.PatientId);

        if (!patientExists)
        {
            return NotFound("Patient not found.");
        }

        // Create a VitalSign entity from the data received in the request.
        var vitalSign = new VitalSign
        {
            PatientId = request.PatientId,
            HeartRate = request.HeartRate,
            SystolicBloodPressure = request.SystolicBloodPressure,
            DiastolicBloodPressure = request.DiastolicBloodPressure,
            Temperature = request.Temperature,
            OxygenSaturation = request.OxygenSaturation,
            RecordedAt = request.RecordedAt
        };

        // Add the new vital sign to the database.
        await _context.VitalSigns.AddAsync(vitalSign);

        // Save the changes and generate the database ID.
        await _context.SaveChangesAsync();

        // Map the saved entity to a response DTO.
        var response = new VitalSignResponse
        {
            Id = vitalSign.Id,
            PatientId = vitalSign.PatientId,
            HeartRate = vitalSign.HeartRate,
            SystolicBloodPressure = vitalSign.SystolicBloodPressure,
            DiastolicBloodPressure = vitalSign.DiastolicBloodPressure,
            Temperature = vitalSign.Temperature,
            OxygenSaturation = vitalSign.OxygenSaturation,
            RecordedAt = vitalSign.RecordedAt
        };

        // Return 201 Created with the created resource.
        return CreatedAtAction(
            nameof(GetById),
            new { id = vitalSign.Id },
            response
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // Retrieve all vital signs and project them directly into response DTOs.
        var vitalSigns = await _context.VitalSigns
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

        return Ok(vitalSigns);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        // Find a vital sign by its ID and map it to a response DTO.
        var vitalSign = await _context.VitalSigns
            .Where(v => v.Id == id)
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

        // Return 404 if the requested vital sign does not exist.
        if (vitalSign == null)
        {
            return NotFound();
        }

        return Ok(vitalSign);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateVitalSignRequest request)
    {
        // Find the existing vital sign before updating it.
        var vitalSign = await _context.VitalSigns
            .FirstOrDefaultAsync(v => v.Id == id);

        if (vitalSign == null)
        {
            return NotFound();
        }

        // Update the entity with the new values from the request.
        vitalSign.HeartRate = request.HeartRate;
        vitalSign.SystolicBloodPressure = request.SystolicBloodPressure;
        vitalSign.DiastolicBloodPressure = request.DiastolicBloodPressure;
        vitalSign.Temperature = request.Temperature;
        vitalSign.OxygenSaturation = request.OxygenSaturation;
        vitalSign.RecordedAt = request.RecordedAt;

        // Save the updated values to the database.
        await _context.SaveChangesAsync();

        // Return the updated vital sign as a response DTO.
        return Ok(new VitalSignResponse
        {
            Id = vitalSign.Id,
            PatientId = vitalSign.PatientId,
            HeartRate = vitalSign.HeartRate,
            SystolicBloodPressure = vitalSign.SystolicBloodPressure,
            DiastolicBloodPressure = vitalSign.DiastolicBloodPressure,
            Temperature = vitalSign.Temperature,
            OxygenSaturation = vitalSign.OxygenSaturation,
            RecordedAt = vitalSign.RecordedAt
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        // Find the vital sign that should be deleted.
        var vitalSign = await _context.VitalSigns
            .FirstOrDefaultAsync(v => v.Id == id);

        if (vitalSign == null)
        {
            return NotFound();
        }

        // Mark the entity for deletion.
        _context.VitalSigns.Remove(vitalSign);

        // Permanently apply the deletion to the database.
        await _context.SaveChangesAsync();

        // Return 204 because the resource was successfully deleted.
        return NoContent();
    }
}

