using FinanceManager.Application.DTOs;
using FinanceManager.Application.Enums;
using FinanceManager.Application.Interfaces.Repositories;
using FinanceManager.Application.Interfaces.Services;
using FinanceManager.Core.Entities;
using FinanceManager.Core.Enums;

namespace FinanceManager.Application.Services;

public class ExpenseService : IExpenseService
{
	private readonly ITransactionRepository transactionRepository;
	private readonly ICategoryRepository categoryRepository;
	private readonly IBudgetRepository budgetRepository;

	public ExpenseService(
		ITransactionRepository transactionRepository,
		ICategoryRepository categoryRepository,
		IBudgetRepository budgetRepository)
	{
		this.transactionRepository = transactionRepository;
		this.categoryRepository = categoryRepository;
		this.budgetRepository = budgetRepository;
	}

	public async Task<TransactionDto> CreateAsync(int userId, CreateTransactionRequest request)
	{
		var category = await ValidateExpenseCategoryAsync(userId, request.CategoryId);
		ValidateAmountAndDate(request.Amount, request.TransactionDate);

		var warning = await GetBudgetWarningAsync(userId, request.CategoryId, request.Amount, request.TransactionDate);

		var transaction = new Transaction
		{
			Amount = request.Amount,
			TransactionDate = request.TransactionDate,
			Description = request.Description ?? string.Empty,
			Type = TransactionType.Expense,
			CategoryId = category.CategoryId,
			UserId = userId
		};

		await transactionRepository.CreateAsync(transaction);
		transaction.Category = category;

		return TransactionMapper.ToDto(transaction, warning);
	}

	public async Task<TransactionDto?> GetByIdAsync(int userId, int transactionId)
	{
		var transaction = await transactionRepository.GetByIdAsync(transactionId);
		if (transaction == null || transaction.UserId != userId || transaction.Type != TransactionType.Expense)
		{
			return null;
		}

		return TransactionMapper.ToDto(transaction);
	}

	public async Task<IReadOnlyList<TransactionDto>> GetAllAsync(
		int userId,
		string? keyword = null,
		ExpenseSortField sortBy = ExpenseSortField.Date,
		SortDirection sortDirection = SortDirection.Desc)
	{
		var expenses = (await transactionRepository.GetByUserAndTypeAsync(userId, TransactionType.Expense)).ToList();

		if (!string.IsNullOrWhiteSpace(keyword))
		{
			var key = keyword.Trim();
			expenses = expenses
				.Where(e =>
					e.Description.Contains(key, StringComparison.OrdinalIgnoreCase) ||
					(e.Category?.Name?.Contains(key, StringComparison.OrdinalIgnoreCase) ?? false))
				.ToList();
		}

		expenses = ApplySort(expenses, sortBy, sortDirection);
		return expenses.Select(e => TransactionMapper.ToDto(e)).ToList();
	}

	public async Task<TransactionDto?> UpdateAsync(int userId, int transactionId, UpdateTransactionRequest request)
	{
		var transaction = await transactionRepository.GetByIdAsync(transactionId);
		if (transaction == null || transaction.UserId != userId || transaction.Type != TransactionType.Expense)
		{
			return null;
		}

		var category = await ValidateExpenseCategoryAsync(userId, request.CategoryId);
		ValidateAmountAndDate(request.Amount, request.TransactionDate);

		var warning = await GetBudgetWarningAsync(userId, request.CategoryId, request.Amount, request.TransactionDate);

		transaction.Amount = request.Amount;
		transaction.TransactionDate = request.TransactionDate;
		transaction.Description = request.Description ?? string.Empty;
		transaction.CategoryId = category.CategoryId;
		transaction.Category = category;

		await transactionRepository.UpdateAsync(transaction);
		return TransactionMapper.ToDto(transaction, warning);
	}

	public async Task<bool> DeleteAsync(int userId, int transactionId)
	{
		var transaction = await transactionRepository.GetByIdAsync(transactionId);
		if (transaction == null || transaction.UserId != userId || transaction.Type != TransactionType.Expense)
		{
			return false;
		}

		await transactionRepository.DeleteAsync(transactionId);
		return true;
	}

	private async Task<Category> ValidateExpenseCategoryAsync(int userId, int categoryId)
	{
		var category = await categoryRepository.GetByIdAsync(categoryId);
		if (category == null || category.UserId != userId || category.Type != TransactionType.Expense)
		{
			throw new ArgumentException("Invalid expense category.");
		}

		return category;
	}

	private static void ValidateAmountAndDate(decimal amount, DateTime date)
	{
		if (amount <= 0)
		{
			throw new ArgumentException("Amount must be greater than zero.");
		}

		if (date == default)
		{
			throw new ArgumentException("Transaction date is required.");
		}
	}

	private async Task<string?> GetBudgetWarningAsync(int userId, int categoryId, decimal amount, DateTime date)
	{
		var budget = await budgetRepository.GetByCategoryAndMonthAsync(categoryId, date.Year, date.Month);
		if (budget == null || budget.UserId != userId)
		{
			return null;
		}

		var monthExpenses = await transactionRepository.GetByUserAndTypeAsync(userId, TransactionType.Expense);
		var spent = monthExpenses
			.Where(t => t.CategoryId == categoryId && t.TransactionDate.Year == date.Year && t.TransactionDate.Month == date.Month)
			.Sum(t => t.Amount);

		if (spent + amount > budget.MonthlyLimit)
		{
			return $"Warning: This expense exceeds the monthly budget of {budget.MonthlyLimit:C}.";
		}

		return null;
	}

	private static List<Transaction> ApplySort(List<Transaction> expenses, ExpenseSortField sortBy, SortDirection direction)
	{
		return sortBy switch
		{
			ExpenseSortField.Amount => direction == SortDirection.Asc
				? expenses.OrderBy(e => e.Amount).ToList()
				: expenses.OrderByDescending(e => e.Amount).ToList(),
			ExpenseSortField.Category => direction == SortDirection.Asc
				? expenses.OrderBy(e => e.Category?.Name).ToList()
				: expenses.OrderByDescending(e => e.Category?.Name).ToList(),
			_ => direction == SortDirection.Asc
				? expenses.OrderBy(e => e.TransactionDate).ToList()
				: expenses.OrderByDescending(e => e.TransactionDate).ToList()
		};
	}
}
