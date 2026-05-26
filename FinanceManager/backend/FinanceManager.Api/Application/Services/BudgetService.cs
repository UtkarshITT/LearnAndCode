using FinanceManager.Application.DTOs;
using FinanceManager.Application.Interfaces.Repositories;
using FinanceManager.Application.Interfaces.Services;
using FinanceManager.Core.Entities;
using FinanceManager.Core.Enums;

namespace FinanceManager.Application.Services;

public class BudgetService : IBudgetService
{
	private readonly IBudgetRepository budgetRepository;
	private readonly ICategoryRepository categoryRepository;

	public BudgetService(IBudgetRepository budgetRepository, ICategoryRepository categoryRepository)
	{
		this.budgetRepository = budgetRepository;
		this.categoryRepository = categoryRepository;
	}

	public async Task<BudgetDto> CreateAsync(int userId, CreateBudgetRequest request)
	{
		await ValidateRequestAsync(userId, request.CategoryId, request.MonthlyLimit, request.Year, request.Month);

		var existing = await budgetRepository.GetByCategoryAndMonthAsync(request.CategoryId, request.Year, request.Month);
		if (existing != null && existing.UserId == userId)
		{
			throw new InvalidOperationException("Budget already exists for this category and month.");
		}

		var budget = new Budget
		{
			MonthlyLimit = request.MonthlyLimit,
			Year = request.Year,
			Month = request.Month,
			CategoryId = request.CategoryId,
			UserId = userId,
			CreatedAt = DateTime.UtcNow
		};

		await budgetRepository.CreateAsync(budget);
		var category = await categoryRepository.GetByIdAsync(request.CategoryId);
		return ToDto(budget, category?.Name);
	}

	public async Task<BudgetDto?> GetByIdAsync(int userId, int budgetId)
	{
		var budget = await budgetRepository.GetByIdAsync(budgetId);
		if (budget == null || budget.UserId != userId)
		{
			return null;
		}

		return ToDto(budget, budget.Category?.Name);
	}

	public async Task<IReadOnlyList<BudgetDto>> GetAllAsync(int userId)
	{
		var budgets = await budgetRepository.GetAllByUserIdAsync(userId);
		return budgets.Select(b => ToDto(b, b.Category?.Name)).ToList();
	}

	public async Task<BudgetDto?> UpdateAsync(int userId, int budgetId, UpdateBudgetRequest request)
	{
		var budget = await budgetRepository.GetByIdAsync(budgetId);
		if (budget == null || budget.UserId != userId)
		{
			return null;
		}

		await ValidateRequestAsync(userId, request.CategoryId, request.MonthlyLimit, request.Year, request.Month);

		budget.MonthlyLimit = request.MonthlyLimit;
		budget.Year = request.Year;
		budget.Month = request.Month;
		budget.CategoryId = request.CategoryId;

		await budgetRepository.UpdateAsync(budget);
		var category = await categoryRepository.GetByIdAsync(request.CategoryId);
		return ToDto(budget, category?.Name);
	}

	public async Task<bool> DeleteAsync(int userId, int budgetId)
	{
		var budget = await budgetRepository.GetByIdAsync(budgetId);
		if (budget == null || budget.UserId != userId)
		{
			return false;
		}

		await budgetRepository.DeleteAsync(budgetId);
		return true;
	}

	private async Task ValidateRequestAsync(int userId, int categoryId, decimal limit, int year, int month)
	{
		if (limit <= 0)
		{
			throw new ArgumentException("Monthly limit must be greater than zero.");
		}

		if (month < 1 || month > 12)
		{
			throw new ArgumentException("Month must be between 1 and 12.");
		}

		var category = await categoryRepository.GetByIdAsync(categoryId);
		if (category == null || category.UserId != userId || category.Type != TransactionType.Expense)
		{
			throw new ArgumentException("Budget category must be an expense category.");
		}
	}

	private static BudgetDto ToDto(Budget budget, string? categoryName)
	{
		return new BudgetDto
		{
			BudgetId = budget.BudgetId,
			MonthlyLimit = budget.MonthlyLimit,
			Year = budget.Year,
			Month = budget.Month,
			CategoryId = budget.CategoryId,
			CategoryName = categoryName
		};
	}
}
