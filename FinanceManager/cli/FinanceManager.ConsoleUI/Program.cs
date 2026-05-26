using FinanceManager.ConsoleUI;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
	.SetBasePath(AppContext.BaseDirectory)
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.Build();

var host = new AppHost(configuration);
await host.RunAsync();
