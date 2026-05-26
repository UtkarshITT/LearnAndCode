using FinanceManager.Application.DTOs;

namespace FinanceManager.Application.Interfaces.Services;

public interface IBudgetService
{
	Task<BudgetDto> CreateAsync(int userId, CreateBudgetRequest request);

	Task<BudgetDto?> GetByIdAsync(int userId, int budgetId);

	Task<IReadOnlyList<BudgetDto>> GetAllAsync(int userId);

	Task<BudgetDto?> UpdateAsync(int userId, int budgetId, UpdateBudgetRequest request);

	Task<bool> DeleteAsync(int userId, int budgetId);
}
