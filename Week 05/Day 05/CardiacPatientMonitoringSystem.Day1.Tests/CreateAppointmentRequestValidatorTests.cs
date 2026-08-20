using CardiacPatientMonitoringSystem.DTOs.Requests;
using CardiacPatientMonitoringSystem.Validators;

namespace CardiacPatientMonitoringSystem.Day1.Tests;

public class CreateAppointmentRequestValidatorTests
{
    private readonly CreateAppointmentRequestValidator _validator;

    public CreateAppointmentRequestValidatorTests()
    {
        _validator = new CreateAppointmentRequestValidator();
    }

    [Fact]
    public void Validate_WhenRequestIsValid_ReturnsValid()
    {
        // Arrange
        var request = new CreateAppointmentRequest
        {
            PatientId = 1,
            AppointmentDate = DateTime.UtcNow.AddDays(1),
            Reason = "Cardiac follow-up",
            Status = "Scheduled"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WhenPatientIdIsZero_ReturnsInvalid()
    {
        // Arrange
        var request = new CreateAppointmentRequest
        {
            PatientId = 0,
            AppointmentDate = DateTime.UtcNow.AddDays(1),
            Reason = "Cardiac follow-up",
            Status = "Scheduled"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_WhenAppointmentDateIsInThePast_ReturnsInvalid()
    {
        // Arrange
        var request = new CreateAppointmentRequest
        {
            PatientId = 1,
            AppointmentDate = DateTime.UtcNow.AddDays(-1),
            Reason = "Cardiac follow-up",
            Status = "Scheduled"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_WhenReasonIsEmpty_ReturnsInvalid()
    {
        // Arrange
        var request = new CreateAppointmentRequest
        {
            PatientId = 1,
            AppointmentDate = DateTime.UtcNow.AddDays(1),
            Reason = "",
            Status = "Scheduled"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_WhenStatusIsEmpty_ReturnsInvalid()
    {
        // Arrange
        var request = new CreateAppointmentRequest
        {
            PatientId = 1,
            AppointmentDate = DateTime.UtcNow.AddDays(1),
            Reason = "Cardiac follow-up",
            Status = ""
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
    }
}