using FinanceManager.Application.Interfaces.Repositories;
using FinanceManager.Core.Entities;
using FinanceManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
	private readonly FinanceManagerDbContext dbContext;

	public UserRepository(FinanceManagerDbContext dbContext)
	{
		this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
	}

	public async Task<User?> GetByUsernameAsync(string username)
	{
		return await dbContext.Users
			.IgnoreQueryFilters()
			.FirstOrDefaultAsync(u => u.Username == username && !u.IsDeleted);
	}

	public async Task<User?> GetByIdAsync(int userId)
	{
		return await dbContext.Users
			.FirstOrDefaultAsync(u => u.UserId == userId);
	}

	public async Task<int> CreateAsync(User user)
	{
		dbContext.Users.Add(user);
		await dbContext.SaveChangesAsync();
		return user.UserId;
	}

	public async Task UpdateAsync(User user)
	{
		dbContext.Users.Update(user);
		await dbContext.SaveChangesAsync();
	}
}
