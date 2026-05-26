using FinanceManager.Core.Enums;

namespace FinanceManager.Application.DTOs;

public class TransactionDto
{
	public int TransactionId { get; set; }

	public decimal Amount { get; set; }

	public DateTime TransactionDate { get; set; }

	public string Description { get; set; } = string.Empty;

	public TransactionType Type { get; set; }

	public int CategoryId { get; set; }

	public string? CategoryName { get; set; }

	public string? BudgetWarning { get; set; }
}
