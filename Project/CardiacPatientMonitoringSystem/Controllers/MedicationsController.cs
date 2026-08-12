
using CardiacPatientMonitoringSystem.Data;
using CardiacPatientMonitoringSystem.DTOs.Requests;
using CardiacPatientMonitoringSystem.DTOs.Responses;
using CardiacPatientMonitoringSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoringSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicationsController : ControllerBase
{
    private readonly AppDbContext _context;

    public MedicationsController(AppDbContext context)
    {
        _context = context;
    }

    // Create a new medication for an existing patient.
    [HttpPost]
    public async Task<IActionResult> Create(CreateMedicationRequest request)
    {
        // Check that the patient exists before creating the medication.
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Id == request.PatientId);

        if (!patientExists)
        {
            return NotFound("Patient not found.");
        }

        // Map the request DTO to the Medication entity.
        var medication = new Medication
        {
            PatientId = request.PatientId,
            Name = request.Name,
            Dosage = request.Dosage,
            Frequency = request.Frequency,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        await _context.Medications.AddAsync(medication);
        await _context.SaveChangesAsync();

        // Return only the data that should be exposed to the client.
        var response = new MedicationResponse
        {
            Id = medication.Id,
            PatientId = medication.PatientId,
            Name = medication.Name,
            Dosage = medication.Dosage,
            Frequency = medication.Frequency,
            StartDate = medication.StartDate,
            EndDate = medication.EndDate
        };

        return CreatedAtAction(
            nameof(GetById),
            new { id = medication.Id },
            response
        );
    }

    // Get all medications.
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var medications = await _context.Medications
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

        return Ok(medications);
    }

    // Get a specific medication by its ID.
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var medication = await _context.Medications
            .Where(m => m.Id == id)
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

        if (medication == null)
        {
            return NotFound();
        }

        return Ok(medication);
    }

    // Update an existing medication.
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateMedicationRequest request)
    {
        var medication = await _context.Medications
            .FirstOrDefaultAsync(m => m.Id == id);

        if (medication == null)
        {
            return NotFound();
        }

        // Update the editable medication fields.
        medication.Name = request.Name;
        medication.Dosage = request.Dosage;
        medication.Frequency = request.Frequency;
        medication.StartDate = request.StartDate;
        medication.EndDate = request.EndDate;

        await _context.SaveChangesAsync();

        return Ok(new MedicationResponse
        {
            Id = medication.Id,
            PatientId = medication.PatientId,
            Name = medication.Name,
            Dosage = medication.Dosage,
            Frequency = medication.Frequency,
            StartDate = medication.StartDate,
            EndDate = medication.EndDate
        });
    }

    // Delete an existing medication.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var medication = await _context.Medications
            .FirstOrDefaultAsync(m => m.Id == id);

        if (medication == null)
        {
            return NotFound();
        }

        _context.Medications.Remove(medication);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}

