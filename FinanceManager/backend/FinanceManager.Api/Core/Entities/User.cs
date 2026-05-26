using FinanceManager.Core.Interfaces;

namespace FinanceManager.Core.Entities;

public class User : ISoftDeletable
{
	public int UserId { get; set; }

	public string Username { get; set; } = string.Empty;

	public string PasswordHash { get; set; } = string.Empty;

	public DateTime CreatedAt { get; set; }

	public bool IsDeleted { get; set; }

	public DateTime? DeletedAt { get; set; }

	public ICollection<Category> Categories { get; set; } = new List<Category>();

	public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

	public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}
