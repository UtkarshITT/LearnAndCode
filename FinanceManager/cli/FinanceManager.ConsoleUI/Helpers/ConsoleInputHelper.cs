using Spectre.Console;

namespace FinanceManager.ConsoleUI.Helpers;

public static class ConsoleInputHelper
{
	public static string ReadPassword(string prompt)
	{
		AnsiConsole.Markup($"[cyan]{prompt}[/]: ");
		var password = string.Empty;
		ConsoleKeyInfo key;

		do
		{
			key = Console.ReadKey(intercept: true);
			if (key.Key == ConsoleKey.Backspace && password.Length > 0)
			{
				password = password[..^1];
			}
			else if (!char.IsControl(key.KeyChar))
			{
				password += key.KeyChar;
			}
		}
		while (key.Key != ConsoleKey.Enter);

		AnsiConsole.WriteLine();
		return password;
	}

	public static bool Confirm(string message)
	{
		return AnsiConsole.Confirm($"[yellow]{message}[/]");
	}

	public static string Prompt(string label, string? defaultValue = null)
	{
		var prompt = string.IsNullOrEmpty(defaultValue)
			? $"[cyan]{label}[/]"
			: $"[cyan]{label}[/] ([grey]{defaultValue}[/])";

		var input = AnsiConsole.Ask<string>(prompt);
		return string.IsNullOrWhiteSpace(input) && defaultValue != null ? defaultValue : input.Trim();
	}

	public static decimal PromptDecimal(string label, decimal? defaultValue = null)
	{
		while (true)
		{
			var text = Prompt(label, defaultValue?.ToString("F2"));
			if (decimal.TryParse(text, out var value) && value > 0)
			{
				return value;
			}

			AnsiConsole.MarkupLine("[yellow]Invalid amount. Enter a positive number.[/]");
		}
	}

	public static int PromptInt(string label, int? defaultValue = null)
	{
		while (true)
		{
			var text = Prompt(label, defaultValue?.ToString());
			if (int.TryParse(text, out var value))
			{
				return value;
			}

			AnsiConsole.MarkupLine("[yellow]Invalid number.[/]");
		}
	}

	public static DateTime PromptDate(string label, DateTime? defaultValue = null)
	{
		var defaultDate = defaultValue ?? DateTime.Today;
		while (true)
		{
			var text = Prompt(label, defaultDate.ToString("yyyy-MM-dd"));
			if (DateTime.TryParse(text, out var value))
			{
				return value;
			}

			AnsiConsole.MarkupLine("[yellow]Invalid date. Use yyyy-MM-dd.[/]");
		}
	}
}
