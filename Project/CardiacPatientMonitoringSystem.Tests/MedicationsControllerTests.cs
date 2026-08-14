using CardiacPatientMonitoringSystem.Controllers;
using CardiacPatientMonitoringSystem.Data;
using CardiacPatientMonitoringSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardiacPatientMonitoringSystem.Tests;

public class MedicationsControllerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetById_WhenMedicationExists_ReturnsOk()
    {
        // Arrange
        await using var context = CreateContext();

        var medication = new Medication
        {
            Id = 1,
            PatientId = 1,
            Name = "Aspirin",
            Dosage = "100 mg",
            Frequency = "Once daily",
            StartDate = new DateTime(2026, 1, 1),
            EndDate = new DateTime(2026, 2, 1)
        };

        await context.Medications.AddAsync(medication);
        await context.SaveChangesAsync();

        var controller = new MedicationsController(context);

        // Act
        var result = await controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(okResult.Value);

        var response = okResult.Value;

        Assert.Equal(
            "Aspirin",
            response.GetType()
                .GetProperty("Name")!
                .GetValue(response)
        );
    }

    [Fact]
    public async Task GetById_WhenMedicationDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        await using var context = CreateContext();

        var controller = new MedicationsController(context);

        // Act
        var result = await controller.GetById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}