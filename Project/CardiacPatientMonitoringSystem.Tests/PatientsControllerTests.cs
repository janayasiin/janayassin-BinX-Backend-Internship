using CardiacPatientMonitoringSystem.Controllers;
using CardiacPatientMonitoringSystem.Data;
using CardiacPatientMonitoringSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoringSystem.Tests;

public class PatientsControllerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetById_WhenPatientExists_ReturnsOk()
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
            Email = "test@example.com",
            MedicalHistory = "No known history"
        };

        await context.Patients.AddAsync(patient);
        await context.SaveChangesAsync();

        var controller = new PatientsController(context);

        // Act
        var result = await controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(okResult.Value);

        var response = okResult.Value;

        Assert.Equal("Test Patient", response.GetType()
            .GetProperty("FullName")!
            .GetValue(response));
    }

    [Fact]
    public async Task GetById_WhenPatientDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        await using var context = CreateContext();

        var controller = new PatientsController(context);

        // Act
        var result = await controller.GetById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}