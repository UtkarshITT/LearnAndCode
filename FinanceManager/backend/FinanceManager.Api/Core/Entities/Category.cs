using FinanceManager.Core.Enums;
using FinanceManager.Core.Interfaces;

namespace FinanceManager.Core.Entities;

public class Category : ISoftDeletable
{
	public int CategoryId { get; set; }

	public string Name { get; set; } = string.Empty;

	public TransactionType Type { get; set; }

	public decimal? BudgetAmount { get; set; }

	public int UserId { get; set; }

	public bool IsDeleted { get; set; }

	public DateTime? DeletedAt { get; set; }

	public User User { get; set; } = null!;

	public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

	public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}
