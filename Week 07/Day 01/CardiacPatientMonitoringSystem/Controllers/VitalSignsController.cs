using System.Security.Claims;
using CardiacPatientMonitoringSystem.DTOs.Requests;
using CardiacPatientMonitoringSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardiacPatientMonitoringSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Patient")]
public class VitalSignsController : ControllerBase
{
    private readonly IVitalSignRecordService _vitalSignService;

    public VitalSignsController(
        IVitalSignRecordService vitalSignService)
    {
        _vitalSignService = vitalSignService;
    }

    // Patient: Get all his own vital-sign records.
    [HttpGet]
    public async Task<IActionResult> GetMyVitalSigns()
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var vitalSigns =
            await _vitalSignService.GetMyVitalSignsAsync(userId);

        return Ok(vitalSigns);
    }

    // Patient: Get one of his own vital-sign records.
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var vitalSign =
            await _vitalSignService.GetByIdAsync(
                id,
                userId);

        if (vitalSign == null)
        {
            return NotFound();
        }

        return Ok(vitalSign);
    }

    // Patient: Create a vital-sign record for himself.
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateVitalSignRequest request)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var vitalSign =
            await _vitalSignService.CreateAsync(
                request,
                userId);

        if (vitalSign == null)
        {
            return NotFound("Patient profile not found.");
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = vitalSign.Id },
            vitalSign);
    }

    // Patient: Update only his own vital-sign record.
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateVitalSignRequest request)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var vitalSign =
            await _vitalSignService.UpdateAsync(
                id,
                request,
                userId);

        if (vitalSign == null)
        {
            return NotFound();
        }

        return Ok(vitalSign);
    }

    // Patient: Delete only his own vital-sign record.
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
            await _vitalSignService.DeleteAsync(
                id,
                userId);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}