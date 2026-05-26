using FinanceManager.Infrastructure.Services;
using FluentAssertions;

namespace FinanceManager.Tests.Infrastructure.Services;

public class BcryptPasswordHasherTests
{
	private readonly BcryptPasswordHasher passwordHasher;

	public BcryptPasswordHasherTests()
	{
		passwordHasher = new BcryptPasswordHasher();
	}

	[Fact]
	public void HashPassword_WithValidPassword_ReturnsHashedPassword()
	{
		// Arrange
		var password = "MySecurePassword123";

		// Act
		var hash = passwordHasher.HashPassword(password);

		// Assert
		hash.Should().NotBeEmpty();
		hash.Should().NotBe(password);
		hash.Length.Should().BeGreaterThan(20);
	}

	[Fact]
	public void HashPassword_WithSamePassword_ReturnsDifferentHashes()
	{
		// Arrange
		var password = "MySecurePassword123";

		// Act
		var hash1 = passwordHasher.HashPassword(password);
		var hash2 = passwordHasher.HashPassword(password);

		// Assert
		hash1.Should().NotBe(hash2);
	}

	[Fact]
	public void VerifyPassword_WithCorrectPassword_ReturnsTrue()
	{
		// Arrange
		var password = "MySecurePassword123";
		var hash = passwordHasher.HashPassword(password);

		// Act
		var result = passwordHasher.VerifyPassword(password, hash);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void VerifyPassword_WithWrongPassword_ReturnsFalse()
	{
		// Arrange
		var password = "MySecurePassword123";
		var wrongPassword = "WrongPassword";
		var hash = passwordHasher.HashPassword(password);

		// Act
		var result = passwordHasher.VerifyPassword(wrongPassword, hash);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void VerifyPassword_WithEmptyPassword_ReturnsFalse()
	{
		// Arrange
		var password = "MySecurePassword123";
		var hash = passwordHasher.HashPassword(password);

		// Act
		var result = passwordHasher.VerifyPassword(string.Empty, hash);

		// Assert
		result.Should().BeFalse();
	}

	[Theory]
	[InlineData("Password1")]
	[InlineData("P@ssw0rd")]
	[InlineData("LongPasswordWith123Numbers")]
	public void HashPassword_WithDifferentPasswords_AllReturnValidHashes(string password)
	{
		// Act
		var hash = passwordHasher.HashPassword(password);

		// Assert
		hash.Should().NotBeEmpty();
		hash.Should().NotBe(password);
		passwordHasher.VerifyPassword(password, hash).Should().BeTrue();
	}
}
