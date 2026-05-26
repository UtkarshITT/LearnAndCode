using FinanceManager.Application.DTOs;
using FinanceManager.ConsoleUI.Api;
using FinanceManager.ConsoleUI.Helpers;
using FinanceManager.ConsoleUI.Logging;
using Spectre.Console;

namespace FinanceManager.ConsoleUI.Menus;

public class BudgetMenu
{
	private readonly FinanceApiClient apiClient;
	private readonly ColoredConsoleLogger logger;

	public BudgetMenu(FinanceApiClient apiClient, ColoredConsoleLogger logger)
	{
		this.apiClient = apiClient;
		this.logger = logger;
	}

	public async Task RunAsync()
	{
		while (true)
		{
			ConsoleScreen.Clear();
			var choice = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[bold]Budget Management[/]")
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
		var categoryId = ConsoleInputHelper.PromptInt("Expense Category ID");
		var limit = ConsoleInputHelper.PromptDecimal("Monthly limit");
		var year = ConsoleInputHelper.PromptInt("Year", DateTime.Today.Year);
		var month = ConsoleInputHelper.PromptInt("Month (1-12)", DateTime.Today.Month);

		try
		{
			var created = await apiClient.CreateBudgetAsync(new CreateBudgetRequest
			{
				CategoryId = categoryId,
				MonthlyLimit = limit,
				Year = year,
				Month = month
			});

			if (created != null)
			{
				logger.LogInfo($"Budget #{created.BudgetId} created.");
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
			var items = await apiClient.GetBudgetsAsync();
			ConsoleFormatter.PrintBudgets(items);
		}
		catch (Exception ex)
		{
			logger.LogError(ex.Message);
		}
	}

	private async Task UpdateAsync()
	{
		var id = ConsoleInputHelper.PromptInt("Budget ID");
		var categoryId = ConsoleInputHelper.PromptInt("Category ID");
		var limit = ConsoleInputHelper.PromptDecimal("Monthly limit");
		var year = ConsoleInputHelper.PromptInt("Year");
		var month = ConsoleInputHelper.PromptInt("Month");

		try
		{
			await apiClient.UpdateBudgetAsync(id, new UpdateBudgetRequest
			{
				CategoryId = categoryId,
				MonthlyLimit = limit,
				Year = year,
				Month = month
			});
			logger.LogInfo("Budget updated.");
		}
		catch (Exception ex)
		{
			logger.LogError(ex.Message);
		}
	}

	private async Task DeleteAsync()
	{
		var id = ConsoleInputHelper.PromptInt("Budget ID");
		if (!ConsoleInputHelper.Confirm("Delete this budget?"))
		{
			return;
		}

		try
		{
			var ok = await apiClient.DeleteBudgetAsync(id);
			logger.LogInfo(ok ? "Budget deleted." : "Budget not found.");
		}
		catch (Exception ex)
		{
			logger.LogError(ex.Message);
		}
	}
}
