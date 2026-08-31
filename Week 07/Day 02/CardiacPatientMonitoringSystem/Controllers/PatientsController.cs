using System.Security.Claims;
using CardiacPatientMonitoringSystem.DTOs;
using CardiacPatientMonitoringSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardiacPatientMonitoringSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll(
        int page = 1,
        int pageSize = 10,
        string? search = null,
        string? sort = null)
    {
        if (page < 1)
        {
            return BadRequest("Page must be greater than 0.");
        }

        if (pageSize < 1 || pageSize > 100)
        {
            return BadRequest("Page size must be between 1 and 100.");
        }

        var result = await _patientService.GetAllAsync(
            page,
            pageSize,
            search,
            sort);

        return Ok(new
        {
            page,
            pageSize,
            totalCount = result.TotalCount,
            data = result.Patients
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (User.IsInRole("Admin"))
        {
            var patient = await _patientService.GetByIdAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var ownPatient = await _patientService.GetMyProfileAsync(userId);

        if (ownPatient == null)
        {
            return NotFound();
        }

        return Ok(ownPatient);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> Update(
        int id,
        UpdatePatientRequest request)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var patient = await _patientService.UpdateAsync(
            id,
            request,
            userId);

        if (patient == null)
        {
            return NotFound();
        }

        return Ok(patient);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _patientService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}