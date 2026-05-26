using FinanceManager.Core.Entities;

namespace FinanceManager.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
	Task<int> CreateAsync(RefreshToken refreshToken);

	Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);

	Task RevokeAsync(int tokenId);

	Task RevokeAllForUserAsync(int userId);
}
