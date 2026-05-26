using FinanceManager.ConsoleUI.Api;
using FinanceManager.ConsoleUI.Logging;

namespace FinanceManager.ConsoleUI.Commands;

public class LogoutCommand : ICommand
{
	private readonly FinanceApiClient apiClient;
	private readonly ColoredConsoleLogger logger;

	public LogoutCommand(FinanceApiClient apiClient, ColoredConsoleLogger logger)
	{
		this.apiClient = apiClient;
		this.logger = logger;
	}

	public async Task ExecuteAsync()
	{
		await apiClient.LogoutAsync();
		logger.LogInfo("Logged out successfully.");
	}
}
