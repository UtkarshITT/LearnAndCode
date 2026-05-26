using FinanceManager.Application.Interfaces.Repositories;
using FinanceManager.Core.Entities;
using FinanceManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
	private readonly FinanceManagerDbContext dbContext;

	public RefreshTokenRepository(FinanceManagerDbContext dbContext)
	{
		this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
	}

	public async Task<int> CreateAsync(RefreshToken refreshToken)
	{
		dbContext.RefreshTokens.Add(refreshToken);
		await dbContext.SaveChangesAsync();
		return refreshToken.TokenId;
	}

	public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash)
	{
		return await dbContext.RefreshTokens
			.FirstOrDefaultAsync(t => t.TokenHash == tokenHash);
	}

	public async Task RevokeAsync(int tokenId)
	{
		var token = await dbContext.RefreshTokens.FindAsync(tokenId);
		if (token != null)
		{
			token.RevokedAt = DateTime.UtcNow;
			await dbContext.SaveChangesAsync();
		}
	}

	public async Task RevokeAllForUserAsync(int userId)
	{
		var tokens = await dbContext.RefreshTokens
			.Where(t => t.UserId == userId && t.RevokedAt == null)
			.ToListAsync();

		foreach (var token in tokens)
		{
			token.RevokedAt = DateTime.UtcNow;
		}

		await dbContext.SaveChangesAsync();
	}
}
