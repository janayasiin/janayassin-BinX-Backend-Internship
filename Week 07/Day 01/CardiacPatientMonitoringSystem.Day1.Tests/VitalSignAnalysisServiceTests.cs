using CardiacPatientMonitoringSystem.Models;
using CardiacPatientMonitoringSystem.Services;

namespace CardiacPatientMonitoringSystem.Day1.Tests;

public class VitalSignAnalysisServiceTests
{
    [Fact]
    public void Analyze_WhenVitalSignsAreCritical_ReturnsCriticalStatus()
    {
        // Arrange
        var service = new VitalSignAnalysisService();

        var vitalSign = new VitalSign
        {
            HeartRate = 135,
            SystolicBloodPressure = 150,
            DiastolicBloodPressure = 95,
            Temperature = 38.5m,
            OxygenSaturation = 88
        };

        // Act
        var result = service.Analyze(vitalSign);

        // Assert
        Assert.Equal("Critical", result.Status);
    }

    [Fact]
    public void Analyze_WhenVitalSignsAreNormal_ReturnsNormalStatus()
    {
        // Arrange
        var service = new VitalSignAnalysisService();

        var vitalSign = new VitalSign
        {
            HeartRate = 75,
            SystolicBloodPressure = 120,
            DiastolicBloodPressure = 80,
            Temperature = 36.8m,
            OxygenSaturation = 98
        };

        // Act
        var result = service.Analyze(vitalSign);

        // Assert
        Assert.Equal("Normal", result.Status);
        Assert.Empty(result.Alerts);
    }
    [Fact]
    public void Analyze_WhenBloodPressureIsHigh_ReturnsWarningStatus()
    {
        // Arrange
        var service = new VitalSignAnalysisService();

        var vitalSign = new VitalSign
        {
            HeartRate = 75,
            SystolicBloodPressure = 145,
            DiastolicBloodPressure = 85,
            Temperature = 36.8m,
            OxygenSaturation = 98
        };

        // Act
        var result = service.Analyze(vitalSign);

        // Assert
        Assert.Equal("Warning", result.Status);
        Assert.Contains("Blood pressure is high.", result.Alerts);
    }

    [Fact]
    public void Analyze_WhenMultipleCriticalValuesExist_ReturnsAllCriticalAlerts()
    {
        // Arrange
        var service = new VitalSignAnalysisService();

        var vitalSign = new VitalSign
        {
            HeartRate = 135,
            SystolicBloodPressure = 150,
            DiastolicBloodPressure = 95,
            Temperature = 38.5m,
            OxygenSaturation = 88
        };

        // Act
        var result = service.Analyze(vitalSign);

        // Assert
        Assert.Equal("Critical", result.Status);

        Assert.Contains("Heart rate is critically high.", result.Alerts);
        Assert.Contains("Blood pressure is high.", result.Alerts);
        Assert.Contains("Oxygen saturation is critically low.", result.Alerts);
        Assert.Contains("Temperature is high.", result.Alerts);
    }
}