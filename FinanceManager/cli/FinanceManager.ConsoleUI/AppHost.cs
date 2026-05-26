using FinanceManager.ConsoleUI.Api;
using FinanceManager.ConsoleUI.Commands;
using FinanceManager.ConsoleUI.Logging;
using FinanceManager.ConsoleUI.Menus;
using FinanceManager.ConsoleUI.Session;
using Microsoft.Extensions.Configuration;

namespace FinanceManager.ConsoleUI;

public class AppHost
{
	private readonly IConfiguration configuration;

	public AppHost(IConfiguration configuration)
	{
		this.configuration = configuration;
	}

	public async Task RunAsync()
	{
		var baseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:5280";
		var session = new UserSession();
		var logger = new ColoredConsoleLogger();

		var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
		var apiClient = new FinanceApiClient(httpClient, session);

		var runner = new ConsoleMenuRunner(
			session,
			new LoginCommand(apiClient, logger),
			new RegisterCommand(apiClient, logger),
			new LogoutCommand(apiClient, logger),
			new ExpenseMenu(apiClient, session, logger),
			new IncomeMenu(apiClient, session, logger),
			new BudgetMenu(apiClient, logger),
			new SummaryMenu(apiClient, logger),
			logger);

		await runner.RunAsync();
	}
}
