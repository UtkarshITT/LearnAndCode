namespace FinanceManager.Application.DTOs;

public class UpdateTransactionRequest
{
	public decimal Amount { get; set; }

	public DateTime TransactionDate { get; set; }

	public string Description { get; set; } = string.Empty;

	public int CategoryId { get; set; }
}
