using FinanceManager.Core.Enums;
using FinanceManager.Core.Interfaces;

namespace FinanceManager.Core.Entities;

public class Transaction : ISoftDeletable
{
	public int TransactionId { get; set; }

	public decimal Amount { get; set; }

	public DateTime TransactionDate { get; set; }

	public string Description { get; set; } = string.Empty;

	public TransactionType Type { get; set; }

	public int CategoryId { get; set; }

	public Category Category { get; set; } = null!;

	public int UserId { get; set; }

	public bool IsDeleted { get; set; }

	public DateTime? DeletedAt { get; set; }

	public User User { get; set; } = null!;
}
