namespace FinanceManager.Application.DTOs;

public class CategorySummaryDto
{
	public int CategoryId { get; set; }

	public string CategoryName { get; set; } = string.Empty;

	public string Type { get; set; } = string.Empty;

	public decimal TotalAmount { get; set; }
}
