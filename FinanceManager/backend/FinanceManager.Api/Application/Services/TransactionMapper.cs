using FinanceManager.Application.DTOs;
using FinanceManager.Core.Entities;

namespace FinanceManager.Application.Services;

internal static class TransactionMapper
{
	public static TransactionDto ToDto(Transaction transaction, string? budgetWarning = null)
	{
		return new TransactionDto
		{
			TransactionId = transaction.TransactionId,
			Amount = transaction.Amount,
			TransactionDate = transaction.TransactionDate,
			Description = transaction.Description,
			Type = transaction.Type,
			CategoryId = transaction.CategoryId,
			CategoryName = transaction.Category?.Name,
			BudgetWarning = budgetWarning
		};
	}
}
