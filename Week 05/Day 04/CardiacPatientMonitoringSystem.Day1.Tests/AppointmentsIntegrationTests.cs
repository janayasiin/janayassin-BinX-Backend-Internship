using System.Net;
using System.Net.Http.Json;
using CardiacPatientMonitoringSystem.DTOs.Requests;

namespace CardiacPatientMonitoringSystem.Day1.Tests;

public class AppointmentsIntegrationTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AppointmentsIntegrationTests(
        CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateAppointment_WhenPatientExists_ReturnsCreated()
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
        var response = await _client.PostAsJsonAsync(
            "/api/Appointments",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);
    }
    [Fact]
    public async Task CreateAppointment_WhenPatientDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var request = new CreateAppointmentRequest
        {
            PatientId = 99999,
            AppointmentDate = DateTime.UtcNow.AddDays(1),
            Reason = "Cardiac follow-up",
            Status = "Scheduled"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/Appointments",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}