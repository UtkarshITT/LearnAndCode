using FinanceManager.Application.DTOs;
using FinanceManager.Application.Interfaces.Repositories;
using FinanceManager.Application.Interfaces.Services;
using FinanceManager.Core.Enums;

namespace FinanceManager.Application.Services;

public class SummaryService : ISummaryService
{
	private readonly ITransactionRepository transactionRepository;

	public SummaryService(ITransactionRepository transactionRepository)
	{
		this.transactionRepository = transactionRepository;
	}

	public async Task<SummaryDto> GetSummaryAsync(int userId)
	{
		var transactions = (await transactionRepository.GetAllByUserIdAsync(userId)).ToList();
		var income = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
		var expenses = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);

		return new SummaryDto
		{
			TotalIncome = income,
			TotalExpenses = expenses,
			NetBalance = income - expenses
		};
	}

	public async Task<IReadOnlyList<CategorySummaryDto>> GetByCategoryAsync(int userId)
	{
		var transactions = await transactionRepository.GetAllByUserIdAsync(userId);

		return transactions
			.GroupBy(t => new { t.CategoryId, Name = t.Category?.Name ?? "Unknown", Type = t.Type.ToString() })
			.Select(g => new CategorySummaryDto
			{
				CategoryId = g.Key.CategoryId,
				CategoryName = g.Key.Name,
				Type = g.Key.Type,
				TotalAmount = g.Sum(t => t.Amount)
			})
			.OrderByDescending(c => c.TotalAmount)
			.ToList();
	}

	public async Task<decimal> GetBalanceAsync(int userId, DateTime? from = null, DateTime? to = null)
	{
		IEnumerable<Core.Entities.Transaction> transactions;

		if (from.HasValue && to.HasValue)
		{
			transactions = await transactionRepository.GetByDateRangeAsync(userId, from.Value, to.Value);
		}
		else
		{
			transactions = await transactionRepository.GetAllByUserIdAsync(userId);
		}

		var list = transactions.ToList();
		var income = list.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
		var expenses = list.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
		return income - expenses;
	}
}
