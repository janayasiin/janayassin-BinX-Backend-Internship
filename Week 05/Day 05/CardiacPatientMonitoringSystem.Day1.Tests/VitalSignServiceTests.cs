using CardiacPatientMonitoringSystem.Services;

namespace CardiacPatientMonitoringSystem.Day1.Tests;

public class VitalSignServiceTests
{
    [Theory]
    [InlineData(60, true)]
    [InlineData(75, true)]
    [InlineData(100, true)]
    [InlineData(50, false)]
    [InlineData(101, false)]
    public void IsHeartRateNormal_ReturnsExpectedResult(
        int heartRate,
        bool expected)
    {
        // Arrange
        var service = new VitalSignService();

        // Act
        var result = service.IsHeartRateNormal(heartRate);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(95, true)]
    [InlineData(98, true)]
    [InlineData(94, false)]
    public void IsOxygenSaturationNormal_ReturnsExpectedResult(
        decimal oxygenSaturation,
        bool expected)
    {
        // Arrange
        var service = new VitalSignService();

        // Act
        var result = service.IsOxygenSaturationNormal(oxygenSaturation);

        // Assert
        Assert.Equal(expected, result);
    }
}