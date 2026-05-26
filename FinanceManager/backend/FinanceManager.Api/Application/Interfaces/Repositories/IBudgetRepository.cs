using FinanceManager.Core.Entities;

namespace FinanceManager.Application.Interfaces.Repositories;

public interface IBudgetRepository
{
	Task<Budget?> GetByIdAsync(int budgetId);

	Task<Budget?> GetByCategoryAndMonthAsync(int categoryId, int year, int month);

	Task<IEnumerable<Budget>> GetAllByUserIdAsync(int userId);

	Task<IEnumerable<Budget>> GetByUserAndCategoryAsync(int userId, int categoryId);

	Task<int> CreateAsync(Budget budget);

	Task UpdateAsync(Budget budget);

	Task DeleteAsync(int budgetId);
}
