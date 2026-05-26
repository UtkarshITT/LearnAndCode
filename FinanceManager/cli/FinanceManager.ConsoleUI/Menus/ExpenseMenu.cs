using FinanceManager.Application.DTOs;
using FinanceManager.Application.Enums;
using FinanceManager.ConsoleUI.Api;
using FinanceManager.ConsoleUI.Helpers;
using FinanceManager.ConsoleUI.Logging;
using FinanceManager.ConsoleUI.Session;
using Spectre.Console;

namespace FinanceManager.ConsoleUI.Menus;

public class ExpenseMenu
{
	private readonly FinanceApiClient apiClient;
	private readonly UserSession session;
	private readonly ColoredConsoleLogger logger;

	public ExpenseMenu(FinanceApiClient apiClient, UserSession session, ColoredConsoleLogger logger)
	{
		this.apiClient = apiClient;
		this.session = session;
		this.logger = logger;
	}

	public async Task RunAsync()
	{
		while (true)
		{
			ConsoleScreen.Clear();
			var choice = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[bold]Expense Management[/]")
					.AddChoices("Create", "List", "Update", "Delete", "Back"));

			switch (choice)
			{
				case "Create":
					await CreateAsync();
					break;
				case "List":
					await ListAsync();
					break;
				case "Update":
					await UpdateAsync();
					break;
				case "Delete":
					await DeleteAsync();
					break;
				default:
					return;
			}

			AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
			Console.ReadKey(true);
		}
	}

	private async Task CreateAsync()
	{
		session.UseDefaults = AnsiConsole.Confirm("Use default mode for this entry?", false);

		var amount = session.UseDefaults
			? 1m
			: ConsoleInputHelper.PromptDecimal("Amount");

		var date = session.UseDefaults
			? DateTime.Today
			: ConsoleInputHelper.PromptDate("Date (yyyy-MM-dd)", DateTime.Today);

		var categoryId = session.UseDefaults && session.LastCategoryId.HasValue
			? session.LastCategoryId.Value
			: ConsoleInputHelper.PromptInt("Category ID", session.LastCategoryId);

		var description = ConsoleInputHelper.Prompt("Description", "Expense");

		try
		{
			var created = await apiClient.CreateExpenseAsync(new CreateTransactionRequest
			{
				Amount = amount,
				TransactionDate = date,
				CategoryId = categoryId,
				Description = description
			});

			if (created != null)
			{
				session.LastCategoryId = created.CategoryId;
				if (!string.IsNullOrEmpty(created.BudgetWarning))
				{
					logger.LogWarning(created.BudgetWarning);
				}

				logger.LogInfo($"Expense #{created.TransactionId} created.");
			}
		}
		catch (Exception ex)
		{
			logger.LogError(ex.Message);
		}
	}

	private async Task ListAsync()
	{
		var keyword = AnsiConsole.Ask<string>("[cyan]Keyword filter[/] (leave empty for none):", string.Empty);
		var sortBy = AnsiConsole.Prompt(
			new SelectionPrompt<ExpenseSortField>()
				.Title("Sort by")
				.AddChoices(ExpenseSortField.Date, ExpenseSortField.Amount, ExpenseSortField.Category));

		var sortDir = AnsiConsole.Prompt(
			new SelectionPrompt<SortDirection>()
				.Title("Sort direction")
				.AddChoices(SortDirection.Asc, SortDirection.Desc));

		try
		{
			var items = await apiClient.GetExpensesAsync(
				string.IsNullOrWhiteSpace(keyword) ? null : keyword,
				sortBy,
				sortDir);

			ConsoleFormatter.PrintExpenses(items);
		}
		catch (Exception ex)
		{
			logger.LogError(ex.Message);
		}
	}

	private async Task UpdateAsync()
	{
		var id = ConsoleInputHelper.PromptInt("Expense ID to update");
		var amount = ConsoleInputHelper.PromptDecimal("Amount");
		var date = ConsoleInputHelper.PromptDate("Date");
		var categoryId = ConsoleInputHelper.PromptInt("Category ID");
		var description = ConsoleInputHelper.Prompt("Description");

		try
		{
			var updated = await apiClient.UpdateExpenseAsync(id, new UpdateTransactionRequest
			{
				Amount = amount,
				TransactionDate = date,
				CategoryId = categoryId,
				Description = description
			});

			if (updated != null)
			{
				if (!string.IsNullOrEmpty(updated.BudgetWarning))
				{
					logger.LogWarning(updated.BudgetWarning);
				}

				logger.LogInfo("Expense updated.");
			}
		}
		catch (Exception ex)
		{
			logger.LogError(ex.Message);
		}
	}

	private async Task DeleteAsync()
	{
		var id = ConsoleInputHelper.PromptInt("Expense ID to delete");
		if (!ConsoleInputHelper.Confirm("Delete this expense?"))
		{
			logger.LogWarning("Delete cancelled.");
			return;
		}

		try
		{
			var ok = await apiClient.DeleteExpenseAsync(id);
			if (ok)
			{
				logger.LogInfo("Expense deleted.");
			}
			else
			{
				logger.LogWarning("Expense not found.");
			}
		}
		catch (Exception ex)
		{
			logger.LogError(ex.Message);
		}
	}
}
