using FinanceManager.Application.DTOs;

namespace FinanceManager.Application.Interfaces.Services;

public interface IIncomeService
{
	Task<TransactionDto> CreateAsync(int userId, CreateTransactionRequest request);

	Task<TransactionDto?> GetByIdAsync(int userId, int transactionId);

	Task<IReadOnlyList<TransactionDto>> GetAllAsync(int userId);

	Task<TransactionDto?> UpdateAsync(int userId, int transactionId, UpdateTransactionRequest request);

	Task<bool> DeleteAsync(int userId, int transactionId);
}
