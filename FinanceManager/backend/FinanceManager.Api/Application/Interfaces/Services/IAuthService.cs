using FinanceManager.Application.DTOs;

namespace FinanceManager.Application.Interfaces.Services;

public interface IAuthService
{
	Task<AuthTokensDto> RegisterAsync(string username, string password);

	Task<AuthTokensDto> LoginAsync(string username, string password);

	Task<AuthTokensDto> RefreshAsync(string refreshToken);

	Task LogoutAsync(string refreshToken);
}
