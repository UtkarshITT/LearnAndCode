namespace FinanceManager.Application.DTOs;

public class CreateTransactionRequest
{
	public decimal Amount { get; set; }

	public DateTime TransactionDate { get; set; }

	public string Description { get; set; } = string.Empty;

	public int CategoryId { get; set; }
}
