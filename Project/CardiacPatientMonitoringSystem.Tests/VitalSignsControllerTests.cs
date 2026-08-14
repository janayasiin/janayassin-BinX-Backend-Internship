using CardiacPatientMonitoringSystem.Controllers;
using CardiacPatientMonitoringSystem.Data;
using CardiacPatientMonitoringSystem.DTOs.Requests;
using CardiacPatientMonitoringSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoringSystem.Tests;

public class VitalSignsControllerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task Create_WhenPatientExists_ReturnsCreated()
    {
        // Arrange
        await using var context = CreateContext();

        var patient = new Patient
        {
            Id = 1,
            FullName = "Test Patient",
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = Gender.Male,
            PhoneNumber = "0599999999",
            Email = "test@example.com"
        };

        await context.Patients.AddAsync(patient);
        await context.SaveChangesAsync();

        var controller = new VitalSignsController(context);

        var request = new CreateVitalSignRequest
        {
            PatientId = 1,
            HeartRate = 75,
            SystolicBloodPressure = 120,
            DiastolicBloodPressure = 80,
            Temperature = 36.7m,
            OxygenSaturation = 98,
            RecordedAt = DateTime.UtcNow
        };

        // Act
        var result = await controller.Create(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);

        Assert.NotNull(createdResult.Value);
    }

    [Fact]
    public async Task Create_WhenPatientDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        await using var context = CreateContext();

        var controller = new VitalSignsController(context);

        var request = new CreateVitalSignRequest
        {
            PatientId = 999,
            HeartRate = 75,
            SystolicBloodPressure = 120,
            DiastolicBloodPressure = 80,
            Temperature = 36.7m,
            OxygenSaturation = 98,
            RecordedAt = DateTime.UtcNow
        };

        // Act
        var result = await controller.Create(request);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal("Patient not found.", notFoundResult.Value);
    }
}