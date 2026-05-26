using FinanceManager.Core.Entities;

namespace FinanceManager.Application.Interfaces.Services;

public interface ITokenService
{
	string GenerateAccessToken(User user);

	string GenerateRefreshToken();

	string HashRefreshToken(string plainToken);

	bool ValidateAccessToken(string token);

	int? GetUserIdFromToken(string token);

	DateTime GetAccessTokenExpiry();
}
