using FinanceManager.Application.DTOs;
using FinanceManager.Application.Interfaces.Repositories;
using FinanceManager.Application.Interfaces.Services;
using FinanceManager.Core.Entities;
using FinanceManager.Core.Enums;

namespace FinanceManager.Application.Services;

public class IncomeService : IIncomeService
{
	private readonly ITransactionRepository transactionRepository;
	private readonly ICategoryRepository categoryRepository;

	public IncomeService(ITransactionRepository transactionRepository, ICategoryRepository categoryRepository)
	{
		this.transactionRepository = transactionRepository;
		this.categoryRepository = categoryRepository;
	}

	public async Task<TransactionDto> CreateAsync(int userId, CreateTransactionRequest request)
	{
		var category = await ValidateIncomeCategoryAsync(userId, request.CategoryId);
		ValidateAmountAndDate(request.Amount, request.TransactionDate);

		var transaction = new Transaction
		{
			Amount = request.Amount,
			TransactionDate = request.TransactionDate,
			Description = request.Description ?? string.Empty,
			Type = TransactionType.Income,
			CategoryId = category.CategoryId,
			UserId = userId
		};

		await transactionRepository.CreateAsync(transaction);
		transaction.Category = category;

		return TransactionMapper.ToDto(transaction);
	}

	public async Task<TransactionDto?> GetByIdAsync(int userId, int transactionId)
	{
		var transaction = await transactionRepository.GetByIdAsync(transactionId);
		if (transaction == null || transaction.UserId != userId || transaction.Type != TransactionType.Income)
		{
			return null;
		}

		return TransactionMapper.ToDto(transaction);
	}

	public async Task<IReadOnlyList<TransactionDto>> GetAllAsync(int userId)
	{
		var items = await transactionRepository.GetByUserAndTypeAsync(userId, TransactionType.Income);
		return items.Select(t => TransactionMapper.ToDto(t)).ToList();
	}

	public async Task<TransactionDto?> UpdateAsync(int userId, int transactionId, UpdateTransactionRequest request)
	{
		var transaction = await transactionRepository.GetByIdAsync(transactionId);
		if (transaction == null || transaction.UserId != userId || transaction.Type != TransactionType.Income)
		{
			return null;
		}

		var category = await ValidateIncomeCategoryAsync(userId, request.CategoryId);
		ValidateAmountAndDate(request.Amount, request.TransactionDate);

		transaction.Amount = request.Amount;
		transaction.TransactionDate = request.TransactionDate;
		transaction.Description = request.Description ?? string.Empty;
		transaction.CategoryId = category.CategoryId;
		transaction.Category = category;

		await transactionRepository.UpdateAsync(transaction);
		return TransactionMapper.ToDto(transaction);
	}

	public async Task<bool> DeleteAsync(int userId, int transactionId)
	{
		var transaction = await transactionRepository.GetByIdAsync(transactionId);
		if (transaction == null || transaction.UserId != userId || transaction.Type != TransactionType.Income)
		{
			return false;
		}

		await transactionRepository.DeleteAsync(transactionId);
		return true;
	}

	private async Task<Category> ValidateIncomeCategoryAsync(int userId, int categoryId)
	{
		var category = await categoryRepository.GetByIdAsync(categoryId);
		if (category == null || category.UserId != userId || category.Type != TransactionType.Income)
		{
			throw new ArgumentException("Invalid income category.");
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
}
