using FinanceManager.ConsoleUI.Api;
using FinanceManager.ConsoleUI.Helpers;
using FinanceManager.ConsoleUI.Logging;
using Spectre.Console;

namespace FinanceManager.ConsoleUI.Commands;

public class RegisterCommand : ICommand
{
	private readonly FinanceApiClient apiClient;
	private readonly ColoredConsoleLogger logger;

	public RegisterCommand(FinanceApiClient apiClient, ColoredConsoleLogger logger)
	{
		this.apiClient = apiClient;
		this.logger = logger;
	}

	public async Task ExecuteAsync()
	{
		ConsoleScreen.Clear();
		AnsiConsole.MarkupLine("[bold cyan]Register[/]");
		var username = ConsoleInputHelper.Prompt("Username");
		var password = ConsoleInputHelper.ReadPassword("Password");

		try
		{
			await apiClient.RegisterAsync(username, password);
			logger.LogInfo("Registration successful. You are now logged in.");
		}
		catch (Exception ex)
		{
			logger.LogWarning(ex.Message);
		}
	}
}
