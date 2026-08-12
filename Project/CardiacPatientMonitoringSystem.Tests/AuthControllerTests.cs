using CardiacPatientMonitoringSystem.Controllers;
using CardiacPatientMonitoringSystem.DTOs.Auth;
using CardiacPatientMonitoringSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CardiacPatientMonitoringSystem.Tests;

public class AuthControllerTests
{
    private static Mock<UserManager<ApplicationUser>> CreateUserManagerMock()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();

        return new Mock<UserManager<ApplicationUser>>(
            store.Object,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!
        );
    }

    private static Mock<SignInManager<ApplicationUser>> CreateSignInManagerMock(
        UserManager<ApplicationUser> userManager)
    {
        var contextAccessor =
            new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();

        var claimsFactory =
            new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();

        return new Mock<SignInManager<ApplicationUser>>(
            userManager,
            contextAccessor.Object,
            claimsFactory.Object,
            null!,
            null!,
            null!,
            null!
        );
    }

    [Fact]
    public async Task Login_WhenUserDoesNotExist_ReturnsUnauthorized()
    {
        // Arrange
        var userManager = CreateUserManagerMock();

        userManager
            .Setup(x => x.FindByEmailAsync("missing@example.com"))
            .ReturnsAsync((ApplicationUser?)null);

        var signInManager = CreateSignInManagerMock(userManager.Object);

        var configuration = new Mock<IConfiguration>();

        var controller = new AuthController(
            userManager.Object,
            signInManager.Object,
            configuration.Object
        );

        var request = new LoginRequest
        {
            Email = "missing@example.com",
            Password = "WrongPassword123!"
        };

        // Act
        var result = await controller.Login(request);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);

        signInManager.Verify(
            x => x.CheckPasswordSignInAsync(
                It.IsAny<ApplicationUser>(),
                It.IsAny<string>(),
                false
            ),
            Times.Never
        );
    }

    [Fact]
    public async Task Login_WhenCredentialsAreValid_ReturnsOkWithToken()
    {
        // Arrange
        var userManager = CreateUserManagerMock();

        var user = new ApplicationUser
        {
            Id = "user-123",
            Email = "test@example.com",
            UserName = "test@example.com"
        };

        userManager
            .Setup(x => x.FindByEmailAsync("test@example.com"))
            .ReturnsAsync(user);

        var signInManager = CreateSignInManagerMock(userManager.Object);

        signInManager
            .Setup(x => x.CheckPasswordSignInAsync(
                user,
                "CorrectPassword123!",
                false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        var configuration = new Mock<IConfiguration>();

        configuration
            .Setup(x => x["Jwt:Key"])
            .Returns("ThisIsASecretKeyForTesting123456789");

        configuration
            .Setup(x => x["Jwt:Issuer"])
            .Returns("TestIssuer");

        configuration
            .Setup(x => x["Jwt:Audience"])
            .Returns("TestAudience");

        var controller = new AuthController(
            userManager.Object,
            signInManager.Object,
            configuration.Object
        );

        var request = new LoginRequest
        {
            Email = "test@example.com",
            Password = "CorrectPassword123!"
        };

        // Act
        var result = await controller.Login(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(okResult.Value);

        var tokenProperty = okResult.Value
            .GetType()
            .GetProperty("token");

        Assert.NotNull(tokenProperty);

        var token = tokenProperty.GetValue(okResult.Value) as string;

        Assert.False(string.IsNullOrWhiteSpace(token));
    }
}