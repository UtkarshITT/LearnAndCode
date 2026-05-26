using FinanceManager.Core.Interfaces;

namespace FinanceManager.Core.Entities;

public class Budget : ISoftDeletable
{
	public int BudgetId { get; set; }

	public decimal MonthlyLimit { get; set; }

	public int Year { get; set; }

	public int Month { get; set; }

	public int CategoryId { get; set; }

	public Category Category { get; set; } = null!;

	public int UserId { get; set; }

	public User User { get; set; } = null!;

	public DateTime CreatedAt { get; set; }

	public bool IsDeleted { get; set; }

	public DateTime? DeletedAt { get; set; }
}
