using FinanceManager.Application.DTOs;
using FinanceManager.Application.Enums;

namespace FinanceManager.Application.Interfaces.Services;

public interface IExpenseService
{
	Task<TransactionDto> CreateAsync(int userId, CreateTransactionRequest request);

	Task<TransactionDto?> GetByIdAsync(int userId, int transactionId);

	Task<IReadOnlyList<TransactionDto>> GetAllAsync(
		int userId,
		string? keyword = null,
		ExpenseSortField sortBy = ExpenseSortField.Date,
		SortDirection sortDirection = SortDirection.Desc);

	Task<TransactionDto?> UpdateAsync(int userId, int transactionId, UpdateTransactionRequest request);

	Task<bool> DeleteAsync(int userId, int transactionId);
}
