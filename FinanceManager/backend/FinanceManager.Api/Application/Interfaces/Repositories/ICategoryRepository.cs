using FinanceManager.Core.Entities;

namespace FinanceManager.Application.Interfaces.Repositories;

public interface ICategoryRepository
{
	Task<Category?> GetByIdAsync(int categoryId);

	Task<IEnumerable<Category>> GetAllByUserIdAsync(int userId);

	Task<int> CreateAsync(Category category);

	Task UpdateAsync(Category category);

	Task DeleteAsync(int categoryId);
}
