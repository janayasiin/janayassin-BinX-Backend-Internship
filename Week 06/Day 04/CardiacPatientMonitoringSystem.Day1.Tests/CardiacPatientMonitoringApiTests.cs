using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
namespace CardiacPatientMonitoringSystem.Day1.Tests;

public class CardiacPatientMonitoringApiTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;
    private readonly CustomWebApplicationFactory _factory;

    public CardiacPatientMonitoringApiTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;

        _client = factory.CreateClient();
        _configuration = factory.Configuration;

        var token = CreateTestToken();

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    private string CreateTestToken()
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!
            )
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user"),
            new Claim(ClaimTypes.Email, "test@example.com")
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [Fact]
    public async Task GetPatients_ReturnsPatientsSuccessfully()
    {
        var response = await _client.GetAsync("/api/Patients");

        Assert.True(
            response.IsSuccessStatusCode,
            $"API returned status code: {(int)response.StatusCode} {response.StatusCode}"
        );

        var content = await response.Content.ReadAsStringAsync();

        Assert.False(
            string.IsNullOrWhiteSpace(content),
            "API response body should not be empty."
        );

        var patients = JsonSerializer.Deserialize<JsonElement>(content);

        Assert.Equal(JsonValueKind.Array, patients.ValueKind);
    }
    [Fact]
    public async Task GetPatientById_ReturnsNotFound_WhenPatientDoesNotExist()
    {
        var response = await _client.GetAsync("/api/Patients/99999");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode
        );
    }
    [Fact]
    public async Task TestError_ReturnsProblemDetailsWithoutLeakingException()
    {
        // Act
        var response = await _client.GetAsync("/api/Test/error");

        // Assert
        Assert.Equal(
            HttpStatusCode.InternalServerError,
            response.StatusCode
        );

        Assert.Equal(
            "application/problem+json",
            response.Content.Headers.ContentType?.MediaType
        );

        var content = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain(
            "This is a secret internal error.",
            content
        );

        Assert.DoesNotContain(
            "System.Exception",
            content
        );

        Assert.Contains(
            "An unexpected error occurred.",
            content
        );
    }
    [Fact]
    public async Task GetPatients_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        using var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/Patients");

        // Assert
        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode
        );
    }
}
