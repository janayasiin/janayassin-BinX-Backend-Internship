using System.Security.Claims;
using CardiacPatientMonitoringSystem.DTOs.Requests;
using CardiacPatientMonitoringSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardiacPatientMonitoringSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Patient")]
public class MedicationsController : ControllerBase
{
    private readonly IMedicationService _medicationService;

    public MedicationsController(
        IMedicationService medicationService)
    {
        _medicationService = medicationService;
    }

    // Patient: Get all his own medications.
    [HttpGet]
    public async Task<IActionResult> GetMyMedications()
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var medications =
            await _medicationService.GetMyMedicationsAsync(userId);

        return Ok(medications);
    }

    // Patient: Get one of his own medications.
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var medication =
            await _medicationService.GetByIdAsync(
                id,
                userId);

        if (medication == null)
        {
            return NotFound();
        }

        return Ok(medication);
    }

    // Patient: Add a medication for himself.
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateMedicationRequest request)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var medication =
            await _medicationService.CreateAsync(
                request,
                userId);

        if (medication == null)
        {
            return NotFound("Patient profile not found.");
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = medication.Id },
            medication);
    }

    // Patient: Update his own medication.
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateMedicationRequest request)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var medication =
            await _medicationService.UpdateAsync(
                id,
                request,
                userId);

        if (medication == null)
        {
            return NotFound();
        }

        return Ok(medication);
    }

    // Patient: Delete his own medication.
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
            await _medicationService.DeleteAsync(
                id,
                userId);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}