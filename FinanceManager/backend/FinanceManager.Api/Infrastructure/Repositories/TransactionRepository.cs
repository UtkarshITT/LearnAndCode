using FinanceManager.Application.Interfaces.Repositories;
using FinanceManager.Core.Entities;
using FinanceManager.Core.Enums;
using FinanceManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
	private readonly FinanceManagerDbContext dbContext;

	public TransactionRepository(FinanceManagerDbContext dbContext)
	{
		this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
	}

	public async Task<Transaction?> GetByIdAsync(int transactionId)
	{
		return await dbContext.Transactions
			.Include(t => t.Category)
			.FirstOrDefaultAsync(t => t.TransactionId == transactionId);
	}

	public async Task<IEnumerable<Transaction>> GetAllByUserIdAsync(int userId)
	{
		return await dbContext.Transactions
			.Where(t => t.UserId == userId)
			.Include(t => t.Category)
			.OrderByDescending(t => t.TransactionDate)
			.ToListAsync();
	}

	public async Task<IEnumerable<Transaction>> GetByUserAndTypeAsync(int userId, TransactionType type)
	{
		return await dbContext.Transactions
			.Where(t => t.UserId == userId && t.Type == type)
			.Include(t => t.Category)
			.OrderByDescending(t => t.TransactionDate)
			.ToListAsync();
	}

	public async Task<IEnumerable<Transaction>> GetByUserAndCategoryAsync(int userId, int categoryId)
	{
		return await dbContext.Transactions
			.Where(t => t.UserId == userId && t.CategoryId == categoryId)
			.Include(t => t.Category)
			.OrderByDescending(t => t.TransactionDate)
			.ToListAsync();
	}

	public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
	{
		return await dbContext.Transactions
			.Where(t => t.UserId == userId && t.TransactionDate >= startDate && t.TransactionDate <= endDate)
			.Include(t => t.Category)
			.OrderByDescending(t => t.TransactionDate)
			.ToListAsync();
	}

	public async Task<int> CreateAsync(Transaction transaction)
	{
		dbContext.Transactions.Add(transaction);
		await dbContext.SaveChangesAsync();
		return transaction.TransactionId;
	}

	public async Task UpdateAsync(Transaction transaction)
	{
		dbContext.Transactions.Update(transaction);
		await dbContext.SaveChangesAsync();
	}

	public async Task DeleteAsync(int transactionId)
	{
		var transaction = await dbContext.Transactions
			.FirstOrDefaultAsync(t => t.TransactionId == transactionId);

		if (transaction != null)
		{
			transaction.IsDeleted = true;
			transaction.DeletedAt = DateTime.UtcNow;
			await dbContext.SaveChangesAsync();
		}
	}
}
