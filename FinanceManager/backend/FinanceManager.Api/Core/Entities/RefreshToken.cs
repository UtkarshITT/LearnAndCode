namespace FinanceManager.Core.Entities;

public class RefreshToken
{
	public int TokenId { get; set; }

	public int UserId { get; set; }

	public User User { get; set; } = null!;

	public string TokenHash { get; set; } = string.Empty;

	public DateTime ExpiresAt { get; set; }

	public DateTime? RevokedAt { get; set; }

	public DateTime CreatedAt { get; set; }
}
