using FinanceManager.Application.Interfaces.Repositories;
using FinanceManager.Core.Entities;
using FinanceManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
	private readonly FinanceManagerDbContext dbContext;

	public CategoryRepository(FinanceManagerDbContext dbContext)
	{
		this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
	}

	public async Task<Category?> GetByIdAsync(int categoryId)
	{
		return await dbContext.Categories
			.FirstOrDefaultAsync(c => c.CategoryId == categoryId);
	}

	public async Task<IEnumerable<Category>> GetAllByUserIdAsync(int userId)
	{
		return await dbContext.Categories
			.Where(c => c.UserId == userId)
			.ToListAsync();
	}

	public async Task<int> CreateAsync(Category category)
	{
		dbContext.Categories.Add(category);
		await dbContext.SaveChangesAsync();
		return category.CategoryId;
	}

	public async Task UpdateAsync(Category category)
	{
		dbContext.Categories.Update(category);
		await dbContext.SaveChangesAsync();
	}

	public async Task DeleteAsync(int categoryId)
	{
		var category = await dbContext.Categories
			.FirstOrDefaultAsync(c => c.CategoryId == categoryId);

		if (category != null)
		{
			category.IsDeleted = true;
			category.DeletedAt = DateTime.UtcNow;
			await dbContext.SaveChangesAsync();
		}
	}
}
