using CardiacPatientMonitoringSystem.Services;

namespace CardiacPatientMonitoringSystem.Day1.Tests;

public class VitalSignServiceTests
{
    // =========================
    // Fact Tests
    // =========================

    [Fact]
    public void IsHeartRateNormal_WhenHeartRateIs60_ReturnsTrue()
    {
        // Arrange
        var service = new VitalSignService();

        // Act
        var result = service.IsHeartRateNormal(60);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsHeartRateNormal_WhenHeartRateIs50_ReturnsFalse()
    {
        // Arrange
        var service = new VitalSignService();

        // Act
        var result = service.IsHeartRateNormal(50);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsOxygenSaturationNormal_WhenOxygenSaturationIs98_ReturnsTrue()
    {
        // Arrange
        var service = new VitalSignService();

        // Act
        var result = service.IsOxygenSaturationNormal(98);

        // Assert
        Assert.True(result);
    }


    // =========================
    // Theory Tests
    // =========================

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