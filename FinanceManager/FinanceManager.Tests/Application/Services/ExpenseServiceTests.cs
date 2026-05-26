using FinanceManager.Application.DTOs;
using FinanceManager.Application.Services;
using FinanceManager.Application.Interfaces.Repositories;
using FinanceManager.Core.Entities;
using FinanceManager.Core.Enums;
using FluentAssertions;
using Moq;

namespace FinanceManager.Tests.Application.Services;

public class ExpenseServiceTests
{
	private readonly Mock<ITransactionRepository> transactionRepositoryMock;
	private readonly Mock<ICategoryRepository> categoryRepositoryMock;
	private readonly Mock<IBudgetRepository> budgetRepositoryMock;
	private readonly ExpenseService expenseService;

	public ExpenseServiceTests()
	{
		transactionRepositoryMock = new Mock<ITransactionRepository>();
		categoryRepositoryMock = new Mock<ICategoryRepository>();
		budgetRepositoryMock = new Mock<IBudgetRepository>();
		expenseService = new ExpenseService(
			transactionRepositoryMock.Object,
			categoryRepositoryMock.Object,
			budgetRepositoryMock.Object);
	}

	[Fact]
	public async Task CreateAsync_WithInvalidAmount_ThrowsArgumentException()
	{
		categoryRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Category
		{
			CategoryId = 1,
			UserId = 1,
			Type = TransactionType.Expense,
			Name = "Food"
		});

		var request = new CreateTransactionRequest
		{
			Amount = 0,
			CategoryId = 1,
			TransactionDate = DateTime.Today
		};

		await expenseService.Invoking(s => s.CreateAsync(1, request))
			.Should().ThrowAsync<ArgumentException>();
	}

	[Fact]
	public async Task CreateAsync_WithValidRequest_ReturnsTransactionDto()
	{
		var category = new Category { CategoryId = 1, UserId = 1, Type = TransactionType.Expense, Name = "Food" };
		categoryRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
		budgetRepositoryMock.Setup(r => r.GetByCategoryAndMonthAsync(1, It.IsAny<int>(), It.IsAny<int>()))
			.ReturnsAsync((Budget?)null);
		transactionRepositoryMock.Setup(r => r.GetByUserAndTypeAsync(1, TransactionType.Expense))
			.ReturnsAsync(Array.Empty<Transaction>());
		transactionRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Transaction>()))
			.Callback<Transaction>(t => t.TransactionId = 10)
			.ReturnsAsync(10);

		var request = new CreateTransactionRequest
		{
			Amount = 50,
			CategoryId = 1,
			TransactionDate = DateTime.Today,
			Description = "Lunch"
		};

		var result = await expenseService.CreateAsync(1, request);

		result.TransactionId.Should().Be(10);
		result.Amount.Should().Be(50);
	}
}
