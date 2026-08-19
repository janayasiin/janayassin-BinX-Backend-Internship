using CardiacPatientMonitoringSystem.Services;

namespace CardiacPatientMonitoringSystem.Day1.Tests;

public class VitalSignServiceTests
{
    [Fact]
    public void IsHeartRateNormal_WhenValueIsNormal_ReturnsTrue()
    {
        // Arrange
        var service = new VitalSignService();
        // Act
        var result = service.IsHeartRateNormal(75);
        Assert.True(result);
    }

    [Fact]
    public void IsHeartRateNormal_WhenValueIsTooLow_ReturnsFalse()
    {
        var service = new VitalSignService();
        // Act
        var result = service.IsHeartRateNormal(50);
        // Assert
        Assert.False(result);


    }
    [Fact]
    public void IsHeartRateNormal_WhenValueIs100_ReturnsTrue()
    {
        // Arrange
        var service = new VitalSignService();

        // Act
        var result = service.IsHeartRateNormal(100);

        // Assert
        Assert.True(result);

    }

    [Theory]
    [InlineData(60, true)]
    [InlineData(75, true)]
    [InlineData(50, false)]
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

}