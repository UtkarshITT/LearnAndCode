using FinanceManager.Core.Entities;
using FinanceManager.Core.Enums;

namespace FinanceManager.Application.Interfaces.Repositories;

public interface ITransactionRepository
{
	Task<Transaction?> GetByIdAsync(int transactionId);

	Task<IEnumerable<Transaction>> GetAllByUserIdAsync(int userId);

	Task<IEnumerable<Transaction>> GetByUserAndTypeAsync(int userId, TransactionType type);

	Task<IEnumerable<Transaction>> GetByUserAndCategoryAsync(int userId, int categoryId);

	Task<IEnumerable<Transaction>> GetByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);

	Task<int> CreateAsync(Transaction transaction);

	Task UpdateAsync(Transaction transaction);

	Task DeleteAsync(int transactionId);
}
