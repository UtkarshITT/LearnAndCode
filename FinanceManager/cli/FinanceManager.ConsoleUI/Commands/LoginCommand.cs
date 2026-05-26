using FinanceManager.ConsoleUI.Api;
using FinanceManager.ConsoleUI.Helpers;
using FinanceManager.ConsoleUI.Logging;
using Spectre.Console;

namespace FinanceManager.ConsoleUI.Commands;

public class LoginCommand : ICommand
{
	private readonly FinanceApiClient apiClient;
	private readonly ColoredConsoleLogger logger;

	public LoginCommand(FinanceApiClient apiClient, ColoredConsoleLogger logger)
	{
		this.apiClient = apiClient;
		this.logger = logger;
	}

	public async Task ExecuteAsync()
	{
		ConsoleScreen.Clear();
		AnsiConsole.MarkupLine("[bold cyan]Login[/]");
		var username = ConsoleInputHelper.Prompt("Username");
		var password = ConsoleInputHelper.ReadPassword("Password");

		try
		{
			await apiClient.LoginAsync(username, password);
			logger.LogInfo($"Welcome, {username}!");
		}
		catch (Exception ex)
		{
			logger.LogWarning(ex.Message);
		}
	}
}
