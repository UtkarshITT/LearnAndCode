using FinanceManager.Application.DTOs;
using Spectre.Console;

namespace FinanceManager.ConsoleUI.Helpers;

public static class ConsoleFormatter
{
	public static void PrintExpenses(IReadOnlyList<TransactionDto> items)
	{
		if (items.Count == 0)
		{
			AnsiConsole.MarkupLine("[grey]No expenses found.[/]");
			return;
		}

		var table = new Table();
		table.AddColumn("Id");
		table.AddColumn("Date");
		table.AddColumn("Amount");
		table.AddColumn("Category");
		table.AddColumn("Description");

		foreach (var item in items)
		{
			table.AddRow(
				item.TransactionId.ToString(),
				item.TransactionDate.ToString("yyyy-MM-dd"),
				item.Amount.ToString("C"),
				item.CategoryName ?? "-",
				item.Description);
		}

		AnsiConsole.Write(table);
	}

	public static void PrintTransactions(IReadOnlyList<TransactionDto> items, string title)
	{
		AnsiConsole.MarkupLine($"[bold]{title}[/]");
		if (items.Count == 0)
		{
			AnsiConsole.MarkupLine("[grey]No records found.[/]");
			return;
		}

		var table = new Table();
		table.AddColumn("Id");
		table.AddColumn("Date");
		table.AddColumn("Amount");
		table.AddColumn("Category");
		table.AddColumn("Description");

		foreach (var item in items)
		{
			table.AddRow(
				item.TransactionId.ToString(),
				item.TransactionDate.ToString("yyyy-MM-dd"),
				item.Amount.ToString("C"),
				item.CategoryName ?? "-",
				item.Description);
		}

		AnsiConsole.Write(table);
	}

	public static void PrintBudgets(IReadOnlyList<BudgetDto> items)
	{
		if (items.Count == 0)
		{
			AnsiConsole.MarkupLine("[grey]No budgets found.[/]");
			return;
		}

		var table = new Table();
		table.AddColumn("Id");
		table.AddColumn("Category");
		table.AddColumn("Year");
		table.AddColumn("Month");
		table.AddColumn("Limit");

		foreach (var item in items)
		{
			table.AddRow(
				item.BudgetId.ToString(),
				item.CategoryName ?? "-",
				item.Year.ToString(),
				item.Month.ToString(),
				item.MonthlyLimit.ToString("C"));
		}

		AnsiConsole.Write(table);
	}

	public static void PrintSummary(SummaryDto summary)
	{
		var panel = new Panel(
			$"Total Income: {summary.TotalIncome:C}\n" +
			$"Total Expenses: {summary.TotalExpenses:C}\n" +
			$"Net Balance: {summary.NetBalance:C}")
		{
			Header = new PanelHeader("Financial Summary"),
			Border = BoxBorder.Rounded
		};

		AnsiConsole.Write(panel);
	}
}
