using FinanceManager.Application.DTOs;
using FinanceManager.Application.Interfaces.Repositories;
using FinanceManager.Application.Interfaces.Services;
using FinanceManager.Core.Entities;
using FinanceManager.Core.Enums;

namespace FinanceManager.Application.Services;

public class AuthService : IAuthService
{
	private readonly IUserRepository userRepository;
	private readonly ICategoryRepository categoryRepository;
	private readonly IRefreshTokenRepository refreshTokenRepository;
	private readonly IPasswordHasher passwordHasher;
	private readonly ITokenService tokenService;

	public AuthService(
		IUserRepository userRepository,
		ICategoryRepository categoryRepository,
		IRefreshTokenRepository refreshTokenRepository,
		IPasswordHasher passwordHasher,
		ITokenService tokenService)
	{
		this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
		this.categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
		this.refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
		this.passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
		this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
	}

	public async Task<AuthTokensDto> RegisterAsync(string username, string password)
	{
		ValidateCredentials(username, password);

		var existingUser = await userRepository.GetByUsernameAsync(username);
		if (existingUser != null)
		{
			throw new InvalidOperationException("Username already exists.");
		}

		var passwordHash = passwordHasher.HashPassword(password);
		var newUser = new User
		{
			Username = username,
			PasswordHash = passwordHash,
			CreatedAt = DateTime.UtcNow
		};

		await userRepository.CreateAsync(newUser);
		await SeedDefaultCategoriesAsync(newUser.UserId);

		return await IssueTokensAsync(newUser);
	}

	public async Task<AuthTokensDto> LoginAsync(string username, string password)
	{
		ValidateCredentials(username, password);

		var user = await userRepository.GetByUsernameAsync(username);
		if (user == null || !passwordHasher.VerifyPassword(password, user.PasswordHash))
		{
			throw new UnauthorizedAccessException("Invalid username or password.");
		}

		return await IssueTokensAsync(user);
	}

	public async Task<AuthTokensDto> RefreshAsync(string refreshToken)
	{
		if (string.IsNullOrWhiteSpace(refreshToken))
		{
			throw new ArgumentException("Refresh token is required.", nameof(refreshToken));
		}

		var hash = tokenService.HashRefreshToken(refreshToken);
		var stored = await refreshTokenRepository.GetByTokenHashAsync(hash);
		if (stored == null || stored.RevokedAt != null || stored.ExpiresAt <= DateTime.UtcNow)
		{
			throw new UnauthorizedAccessException("Invalid or expired refresh token.");
		}

		var user = await userRepository.GetByIdAsync(stored.UserId);
		if (user == null)
		{
			throw new UnauthorizedAccessException("User not found.");
		}

		await refreshTokenRepository.RevokeAsync(stored.TokenId);
		return await IssueTokensAsync(user);
	}

	public async Task LogoutAsync(string refreshToken)
	{
		if (string.IsNullOrWhiteSpace(refreshToken))
		{
			return;
		}

		var hash = tokenService.HashRefreshToken(refreshToken);
		var stored = await refreshTokenRepository.GetByTokenHashAsync(hash);
		if (stored != null)
		{
			await refreshTokenRepository.RevokeAsync(stored.TokenId);
		}
	}

	private async Task<AuthTokensDto> IssueTokensAsync(User user)
	{
		var accessToken = tokenService.GenerateAccessToken(user);
		var plainRefresh = tokenService.GenerateRefreshToken();
		var refreshEntity = new RefreshToken
		{
			UserId = user.UserId,
			TokenHash = tokenService.HashRefreshToken(plainRefresh),
			ExpiresAt = DateTime.UtcNow.AddDays(7),
			CreatedAt = DateTime.UtcNow
		};

		await refreshTokenRepository.CreateAsync(refreshEntity);

		return new AuthTokensDto
		{
			AccessToken = accessToken,
			RefreshToken = plainRefresh,
			ExpiresAt = tokenService.GetAccessTokenExpiry()
		};
	}

	private async Task SeedDefaultCategoriesAsync(int userId)
	{
		var defaults = new[]
		{
			("Salary", TransactionType.Income),
			("Freelance", TransactionType.Income),
			("Food", TransactionType.Expense),
			("Transport", TransactionType.Expense),
			("Rent", TransactionType.Expense),
			("Utilities", TransactionType.Expense)
		};

		foreach (var (name, type) in defaults)
		{
			await categoryRepository.CreateAsync(new Category
			{
				Name = name,
				Type = type,
				UserId = userId
			});
		}
	}

	private static void ValidateCredentials(string username, string password)
	{
		if (string.IsNullOrWhiteSpace(username))
		{
			throw new ArgumentException("Username cannot be empty.", nameof(username));
		}

		if (string.IsNullOrWhiteSpace(password))
		{
			throw new ArgumentException("Password cannot be empty.", nameof(password));
		}

		if (username.Length < 3 || username.Length > 100)
		{
			throw new ArgumentException("Username must be between 3 and 100 characters.", nameof(username));
		}

		if (password.Length < 6)
		{
			throw new ArgumentException("Password must be at least 6 characters.", nameof(password));
		}
	}
}
