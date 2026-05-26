using FinanceManager.Application.Interfaces.Repositories;
using FinanceManager.Core.Entities;
using FinanceManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Infrastructure.Repositories;

public class BudgetRepository : IBudgetRepository
{
	private readonly FinanceManagerDbContext dbContext;

	public BudgetRepository(FinanceManagerDbContext dbContext)
	{
		this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
	}

	public async Task<Budget?> GetByIdAsync(int budgetId)
	{
		return await dbContext.Budgets
			.Include(b => b.Category)
			.FirstOrDefaultAsync(b => b.BudgetId == budgetId);
	}

	public async Task<Budget?> GetByCategoryAndMonthAsync(int categoryId, int year, int month)
	{
		return await dbContext.Budgets
			.Include(b => b.Category)
			.FirstOrDefaultAsync(b => b.CategoryId == categoryId && b.Year == year && b.Month == month);
	}

	public async Task<IEnumerable<Budget>> GetAllByUserIdAsync(int userId)
	{
		return await dbContext.Budgets
			.Where(b => b.UserId == userId)
			.Include(b => b.Category)
			.OrderByDescending(b => b.Year)
			.ThenByDescending(b => b.Month)
			.ToListAsync();
	}

	public async Task<IEnumerable<Budget>> GetByUserAndCategoryAsync(int userId, int categoryId)
	{
		return await dbContext.Budgets
			.Where(b => b.UserId == userId && b.CategoryId == categoryId)
			.Include(b => b.Category)
			.OrderByDescending(b => b.Year)
			.ThenByDescending(b => b.Month)
			.ToListAsync();
	}

	public async Task<int> CreateAsync(Budget budget)
	{
		dbContext.Budgets.Add(budget);
		await dbContext.SaveChangesAsync();
		return budget.BudgetId;
	}

	public async Task UpdateAsync(Budget budget)
	{
		dbContext.Budgets.Update(budget);
		await dbContext.SaveChangesAsync();
	}

	public async Task DeleteAsync(int budgetId)
	{
		var budget = await dbContext.Budgets
			.FirstOrDefaultAsync(b => b.BudgetId == budgetId);

		if (budget != null)
		{
			budget.IsDeleted = true;
			budget.DeletedAt = DateTime.UtcNow;
			await dbContext.SaveChangesAsync();
		}
	}
}
