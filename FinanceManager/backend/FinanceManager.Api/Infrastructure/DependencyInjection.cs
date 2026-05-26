using FinanceManager.Application.Interfaces.Repositories;
using FinanceManager.Application.Interfaces.Services;
using FinanceManager.Application.Services;
using FinanceManager.Infrastructure.Configuration;
using FinanceManager.Infrastructure.Persistence;
using FinanceManager.Infrastructure.Repositories;
using FinanceManager.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddFinanceManagerInfrastructure(
		this IServiceCollection services,
		Action<DbContextOptionsBuilder> configureDb,
		JwtSettings jwtSettings)
	{
		services.AddSingleton(jwtSettings);

		services.AddDbContext<FinanceManagerDbContext>(configureDb);

		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<ICategoryRepository, CategoryRepository>();
		services.AddScoped<ITransactionRepository, TransactionRepository>();
		services.AddScoped<IBudgetRepository, BudgetRepository>();
		services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

		services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
		services.AddScoped<ITokenService, JwtTokenService>();

		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<IExpenseService, ExpenseService>();
		services.AddScoped<IIncomeService, IncomeService>();
		services.AddScoped<IBudgetService, BudgetService>();
		services.AddScoped<ISummaryService, SummaryService>();

		return services;
	}

	public static IServiceCollection AddFinanceManagerInfrastructure(
		this IServiceCollection services,
		string connectionString,
		JwtSettings jwtSettings)
	{
		return services.AddFinanceManagerInfrastructure(
			options => options.UseSqlServer(connectionString),
			jwtSettings);
	}
}
