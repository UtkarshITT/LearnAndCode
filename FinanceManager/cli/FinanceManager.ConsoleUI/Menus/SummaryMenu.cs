using FinanceManager.ConsoleUI.Api;
using FinanceManager.ConsoleUI.Helpers;
using FinanceManager.ConsoleUI.Logging;
using Spectre.Console;

namespace FinanceManager.ConsoleUI.Menus;

public class SummaryMenu
{
	private readonly FinanceApiClient apiClient;
	private readonly ColoredConsoleLogger logger;

	public SummaryMenu(FinanceApiClient apiClient, ColoredConsoleLogger logger)
	{
		this.apiClient = apiClient;
		this.logger = logger;
	}

	public async Task RunAsync()
	{
		ConsoleScreen.Clear();
		var choice = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title("[bold]Summary[/]")
				.AddChoices("Overview", "By Category", "Back"));

		try
		{
			switch (choice)
			{
				case "Overview":
					var summary = await apiClient.GetSummaryAsync();
					if (summary != null)
					{
						ConsoleFormatter.PrintSummary(summary);
					}
					break;
				case "By Category":
					var byCategory = await apiClient.GetSummaryByCategoryAsync();
					var table = new Table();
					table.AddColumn("Category");
					table.AddColumn("Type");
					table.AddColumn("Total");
					foreach (var row in byCategory)
					{
						table.AddRow(row.CategoryName, row.Type, row.TotalAmount.ToString("C"));
					}
					AnsiConsole.Write(table);
					break;
			}
		}
		catch (Exception ex)
		{
			logger.LogError(ex.Message);
		}

		AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
		Console.ReadKey(true);
	}
}
