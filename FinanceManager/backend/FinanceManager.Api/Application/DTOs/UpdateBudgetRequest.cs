namespace FinanceManager.Application.DTOs;

public class UpdateBudgetRequest
{
	public decimal MonthlyLimit { get; set; }

	public int Year { get; set; }

	public int Month { get; set; }

	public int CategoryId { get; set; }
}
