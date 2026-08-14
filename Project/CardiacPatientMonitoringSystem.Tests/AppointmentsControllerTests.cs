using CardiacPatientMonitoringSystem.Controllers;
using CardiacPatientMonitoringSystem.Data;
using CardiacPatientMonitoringSystem.DTOs.Requests;
using CardiacPatientMonitoringSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoringSystem.Tests;

public class AppointmentsControllerTests
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

        var controller = new AppointmentsController(context);

        var request = new CreateAppointmentRequest
        {
            PatientId = 1,
            AppointmentDate = new DateTime(2026, 8, 20, 10, 0, 0),
            Reason = "Cardiology follow-up",
            Status = "Scheduled"
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

        var controller = new AppointmentsController(context);

        var request = new CreateAppointmentRequest
        {
            PatientId = 999,
            AppointmentDate = new DateTime(2026, 8, 20, 10, 0, 0),
            Reason = "Cardiology follow-up",
            Status = "Scheduled"
        };

        // Act
        var result = await controller.Create(request);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal("Patient not found.", notFoundResult.Value);
    }
}