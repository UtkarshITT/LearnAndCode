using FinanceManager.Application.DTOs;
using FinanceManager.ConsoleUI.Api;
using FinanceManager.ConsoleUI.Helpers;
using FinanceManager.ConsoleUI.Logging;
using FinanceManager.ConsoleUI.Session;
using Spectre.Console;

namespace FinanceManager.ConsoleUI.Menus;

public class IncomeMenu
{
	private readonly FinanceApiClient apiClient;
	private readonly UserSession session;
	private readonly ColoredConsoleLogger logger;

	public IncomeMenu(FinanceApiClient apiClient, UserSession session, ColoredConsoleLogger logger)
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
					.Title("[bold]Income Management[/]")
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
		var amount = session.UseDefaults ? 1m : ConsoleInputHelper.PromptDecimal("Amount");
		var date = session.UseDefaults ? DateTime.Today : ConsoleInputHelper.PromptDate("Date", DateTime.Today);
		var categoryId = session.UseDefaults && session.LastCategoryId.HasValue
			? session.LastCategoryId.Value
			: ConsoleInputHelper.PromptInt("Category ID", session.LastCategoryId);
		var description = ConsoleInputHelper.Prompt("Description", "Income");

		try
		{
			var created = await apiClient.CreateIncomeAsync(new CreateTransactionRequest
			{
				Amount = amount,
				TransactionDate = date,
				CategoryId = categoryId,
				Description = description
			});

			if (created != null)
			{
				session.LastCategoryId = created.CategoryId;
				logger.LogInfo($"Income #{created.TransactionId} created.");
			}
		}
		catch (Exception ex)
		{
			logger.LogError(ex.Message);
		}
	}

	private async Task ListAsync()
	{
		try
		{
			var items = await apiClient.GetIncomeAsync();
			ConsoleFormatter.PrintTransactions(items, "Income");
		}
		catch (Exception ex)
		{
			logger.LogError(ex.Message);
		}
	}

	private async Task UpdateAsync()
	{
		var id = ConsoleInputHelper.PromptInt("Income ID to update");
		var amount = ConsoleInputHelper.PromptDecimal("Amount");
		var date = ConsoleInputHelper.PromptDate("Date");
		var categoryId = ConsoleInputHelper.PromptInt("Category ID");
		var description = ConsoleInputHelper.Prompt("Description");

		try
		{
			await apiClient.UpdateIncomeAsync(id, new UpdateTransactionRequest
			{
				Amount = amount,
				TransactionDate = date,
				CategoryId = categoryId,
				Description = description
			});
			logger.LogInfo("Income updated.");
		}
		catch (Exception ex)
		{
			logger.LogError(ex.Message);
		}
	}

	private async Task DeleteAsync()
	{
		var id = ConsoleInputHelper.PromptInt("Income ID to delete");
		if (!ConsoleInputHelper.Confirm("Delete this income record?"))
		{
			logger.LogWarning("Delete cancelled.");
			return;
		}

		try
		{
			var ok = await apiClient.DeleteIncomeAsync(id);
			logger.LogInfo(ok ? "Income deleted." : "Income not found.");
		}
		catch (Exception ex)
		{
			logger.LogError(ex.Message);
		}
	}
}
