using FinanceManager.Core.Entities;
using FinanceManager.Infrastructure.Configuration;
using FinanceManager.Infrastructure.Services;
using FluentAssertions;

namespace FinanceManager.Tests.Infrastructure.Services;

public class JwtTokenServiceTests
{
	private readonly JwtTokenService tokenService;

	public JwtTokenServiceTests()
	{
		var jwtSettings = new JwtSettings
		{
			SecretKey = "MySuper SecretKeyWith32CharactersMinimum!",
			Issuer = "FinanceManager",
			Audience = "FinanceManagerUsers",
			ExpirationMinutes = 60
		};

		tokenService = new JwtTokenService(jwtSettings);
	}

	[Fact]
	public void GenerateAccessToken_WithValidUser_ReturnsToken()
	{
		var user = new User { UserId = 1, Username = "testuser" };
		var token = tokenService.GenerateAccessToken(user);
		token.Should().NotBeNullOrEmpty();
		token.Should().Contain(".");
	}

	[Fact]
	public void ValidateAccessToken_WithValidToken_ReturnsTrue()
	{
		var user = new User { UserId = 1, Username = "testuser" };
		var token = tokenService.GenerateAccessToken(user);
		tokenService.ValidateAccessToken(token).Should().BeTrue();
	}

	[Fact]
	public void GenerateRefreshToken_ReturnsUniqueValues()
	{
		var t1 = tokenService.GenerateRefreshToken();
		var t2 = tokenService.GenerateRefreshToken();
		t1.Should().NotBe(t2);
	}

	[Fact]
	public void HashRefreshToken_IsDeterministic()
	{
		var hash1 = tokenService.HashRefreshToken("same-token");
		var hash2 = tokenService.HashRefreshToken("same-token");
		hash1.Should().Be(hash2);
	}

	[Fact]
	public void GetUserIdFromToken_WithValidToken_ReturnsCorrectUserId()
	{
		var user = new User { UserId = 42, Username = "testuser" };
		var token = tokenService.GenerateAccessToken(user);
		tokenService.GetUserIdFromToken(token).Should().Be(42);
	}
}
