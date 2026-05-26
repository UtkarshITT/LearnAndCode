using FinanceManager.Core.Entities;

namespace FinanceManager.Application.Interfaces.Repositories;

public interface IUserRepository
{
	Task<User?> GetByUsernameAsync(string username);

	Task<User?> GetByIdAsync(int userId);

	Task<int> CreateAsync(User user);

	Task UpdateAsync(User user);
}
