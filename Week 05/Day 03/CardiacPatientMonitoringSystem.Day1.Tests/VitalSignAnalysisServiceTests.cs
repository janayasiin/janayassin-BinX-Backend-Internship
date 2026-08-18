using CardiacPatientMonitoringSystem.Services;
using Moq;

namespace CardiacPatientMonitoringSystem.Day1.Tests;

public class VitalSignAnalysisServiceTests
{
    [Fact]
    public async Task IsPatientAdultAsync_WhenPatientIsAdult_ReturnsTrue()
    {
        // Arrange
        var mockRepository = new Mock<IPatientRepository>();

        mockRepository
            .Setup(r => r.GetPatientAgeAsync(1))
            .ReturnsAsync(25);

        var service = new VitalSignAnalysisService(mockRepository.Object);

        // Act
        var result = await service.IsPatientAdultAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsPatientAdultAsync_CallsRepositoryOnce()
    {
        // Arrange
        var mockRepository = new Mock<IPatientRepository>();

        mockRepository
            .Setup(r => r.GetPatientAgeAsync(1))
            .ReturnsAsync(25);

        var service = new VitalSignAnalysisService(mockRepository.Object);

        // Act
        await service.IsPatientAdultAsync(1);

        // Assert
        mockRepository.Verify(
            r => r.GetPatientAgeAsync(1),
            Times.Once);
    }
    [Fact]
    public async Task IsPatientAdultAsync_WhenRepositoryThrowsException_ReturnsFalse()
    {
        // Arrange
        var mockRepository = new Mock<IPatientRepository>();

        mockRepository
            .Setup(r => r.GetPatientAgeAsync(1))
            .ThrowsAsync(new Exception("Database error"));

        var service = new VitalSignAnalysisService(mockRepository.Object);

        // Act
        var result = await service.IsPatientAdultAsync(1);

        // Assert
        Assert.False(result);
    }
}
