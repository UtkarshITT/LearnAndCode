using FinanceManager.ConsoleUI.Commands;
using FinanceManager.ConsoleUI.Helpers;
using FinanceManager.ConsoleUI.Logging;
using FinanceManager.ConsoleUI.Menus;
using FinanceManager.ConsoleUI.Session;
using Spectre.Console;

namespace FinanceManager.ConsoleUI;

public class ConsoleMenuRunner
{
	private readonly UserSession session;
	private readonly LoginCommand loginCommand;
	private readonly RegisterCommand registerCommand;
	private readonly LogoutCommand logoutCommand;
	private readonly ExpenseMenu expenseMenu;
	private readonly IncomeMenu incomeMenu;
	private readonly BudgetMenu budgetMenu;
	private readonly SummaryMenu summaryMenu;
	private readonly ColoredConsoleLogger logger;

	public ConsoleMenuRunner(
		UserSession session,
		LoginCommand loginCommand,
		RegisterCommand registerCommand,
		LogoutCommand logoutCommand,
		ExpenseMenu expenseMenu,
		IncomeMenu incomeMenu,
		BudgetMenu budgetMenu,
		SummaryMenu summaryMenu,
		ColoredConsoleLogger logger)
	{
		this.session = session;
		this.loginCommand = loginCommand;
		this.registerCommand = registerCommand;
		this.logoutCommand = logoutCommand;
		this.expenseMenu = expenseMenu;
		this.incomeMenu = incomeMenu;
		this.budgetMenu = budgetMenu;
		this.summaryMenu = summaryMenu;
		this.logger = logger;
	}

	public async Task RunAsync()
	{
		AnsiConsole.Write(new FigletText("Finance Manager").Color(Color.Cyan1));

		while (true)
		{
			ConsoleScreen.Clear();

			if (!session.IsAuthenticated)
			{
				await RunAuthMenuAsync();
				continue;
			}

			await RunMainMenuAsync();
		}
	}

	private async Task RunAuthMenuAsync()
	{
		var choice = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title("[bold]Authentication[/]")
				.AddChoices("Login", "Register", "Exit"));

		switch (choice)
		{
			case "Login":
				await loginCommand.ExecuteAsync();
				break;
			case "Register":
				await registerCommand.ExecuteAsync();
				break;
			case "Exit":
				logger.LogInfo("Goodbye.");
				Environment.Exit(0);
				break;
		}
	}

	private async Task RunMainMenuAsync()
	{
		var choice = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title("[bold]Main Menu[/]")
				.AddChoices("Income", "Expense", "Budget", "Summary", "Logout", "Exit"));

		switch (choice)
		{
			case "Income":
				await incomeMenu.RunAsync();
				break;
			case "Expense":
				await expenseMenu.RunAsync();
				break;
			case "Budget":
				await budgetMenu.RunAsync();
				break;
			case "Summary":
				await summaryMenu.RunAsync();
				break;
			case "Logout":
				await logoutCommand.ExecuteAsync();
				ConsoleScreen.Clear();
				break;
			case "Exit":
				await logoutCommand.ExecuteAsync();
				logger.LogInfo("Goodbye.");
				Environment.Exit(0);
				break;
		}
	}
}
