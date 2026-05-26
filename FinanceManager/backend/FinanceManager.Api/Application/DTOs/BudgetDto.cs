namespace FinanceManager.Application.DTOs;

public class BudgetDto
{
	public int BudgetId { get; set; }

	public decimal MonthlyLimit { get; set; }

	public int Year { get; set; }

	public int Month { get; set; }

	public int CategoryId { get; set; }

	public string? CategoryName { get; set; }
}
