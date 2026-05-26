using Spectre.Console;

namespace FinanceManager.ConsoleUI.Logging;

public class ColoredConsoleLogger
{
	public void LogInfo(string message)
	{
		AnsiConsole.MarkupLine($"[green]{Escape(message)}[/]");
	}

	public void LogWarning(string message)
	{
		AnsiConsole.MarkupLine($"[yellow]{Escape(message)}[/]");
	}

	public void LogError(string message)
	{
		AnsiConsole.MarkupLine($"[red]{Escape(message)}[/]");
	}

	private static string Escape(string message) => message.Replace("[", "[[").Replace("]", "]]");
}
