using FinanceManager.Application.DTOs;
using FinanceManager.Application.Interfaces.Repositories;
using FinanceManager.Application.Interfaces.Services;
using FinanceManager.Application.Services;
using FinanceManager.Core.Entities;
using FluentAssertions;
using Moq;

namespace FinanceManager.Tests.Application.Services;

public class AuthServiceTests
{
	private readonly Mock<IUserRepository> userRepositoryMock;
	private readonly Mock<ICategoryRepository> categoryRepositoryMock;
	private readonly Mock<IRefreshTokenRepository> refreshTokenRepositoryMock;
	private readonly Mock<IPasswordHasher> passwordHasherMock;
	private readonly Mock<ITokenService> tokenServiceMock;
	private readonly AuthService authService;

	public AuthServiceTests()
	{
		userRepositoryMock = new Mock<IUserRepository>();
		categoryRepositoryMock = new Mock<ICategoryRepository>();
		refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
		passwordHasherMock = new Mock<IPasswordHasher>();
		tokenServiceMock = new Mock<ITokenService>();

		authService = new AuthService(
			userRepositoryMock.Object,
			categoryRepositoryMock.Object,
			refreshTokenRepositoryMock.Object,
			passwordHasherMock.Object,
			tokenServiceMock.Object);
	}

	[Fact]
	public async Task RegisterAsync_WithValidCredentials_CreatesUserAndReturnsTokens()
	{
		var username = "newuser";
		var password = "ValidPassword123";

		userRepositoryMock.Setup(r => r.GetByUsernameAsync(username)).ReturnsAsync((User?)null);
		passwordHasherMock.Setup(h => h.HashPassword(password)).Returns("hashed-password");
		tokenServiceMock.Setup(t => t.GenerateAccessToken(It.IsAny<User>())).Returns("access-token");
		tokenServiceMock.Setup(t => t.GenerateRefreshToken()).Returns("refresh-plain");
		tokenServiceMock.Setup(t => t.HashRefreshToken("refresh-plain")).Returns("refresh-hash");
		tokenServiceMock.Setup(t => t.GetAccessTokenExpiry()).Returns(DateTime.UtcNow.AddHours(1));
		userRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<User>())).Callback<User>(u => u.UserId = 1).ReturnsAsync(1);
		categoryRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Category>())).ReturnsAsync(1);
		refreshTokenRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<RefreshToken>())).ReturnsAsync(1);

		var tokens = await authService.RegisterAsync(username, password);

		tokens.AccessToken.Should().Be("access-token");
		tokens.RefreshToken.Should().Be("refresh-plain");
		userRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Once);
		categoryRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Category>()), Times.AtLeastOnce);
	}

	[Fact]
	public async Task LoginAsync_WithValidCredentials_ReturnsTokens()
	{
		var username = "testuser";
		var password = "ValidPassword123";
		var existingUser = new User { UserId = 1, Username = username, PasswordHash = "hashed-password" };

		userRepositoryMock.Setup(r => r.GetByUsernameAsync(username)).ReturnsAsync(existingUser);
		passwordHasherMock.Setup(h => h.VerifyPassword(password, existingUser.PasswordHash)).Returns(true);
		tokenServiceMock.Setup(t => t.GenerateAccessToken(existingUser)).Returns("access-token");
		tokenServiceMock.Setup(t => t.GenerateRefreshToken()).Returns("refresh-plain");
		tokenServiceMock.Setup(t => t.HashRefreshToken("refresh-plain")).Returns("refresh-hash");
		tokenServiceMock.Setup(t => t.GetAccessTokenExpiry()).Returns(DateTime.UtcNow.AddHours(1));
		refreshTokenRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<RefreshToken>())).ReturnsAsync(1);

		var tokens = await authService.LoginAsync(username, password);

		tokens.AccessToken.Should().Be("access-token");
	}

	[Fact]
	public async Task RegisterAsync_WithDuplicateUsername_ThrowsInvalidOperationException()
	{
		var username = "existing-user";
		userRepositoryMock.Setup(r => r.GetByUsernameAsync(username)).ReturnsAsync(new User { UserId = 1, Username = username });

		await authService.Invoking(s => s.RegisterAsync(username, "ValidPassword123"))
			.Should().ThrowAsync<InvalidOperationException>();
	}
}
