using FinanceManager.Infrastructure;
using FinanceManager.Infrastructure.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Tests.Integration;

public class FinanceManagerWebApplicationFactory : WebApplicationFactory<Program>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("Testing");

		builder.ConfigureServices(services =>
		{
			var jwtSettings = new JwtSettings
			{
				SecretKey = "MySuper SecretKeyWith32CharactersMinimum!",
				Issuer = "FinanceManager",
				Audience = "FinanceManagerUsers",
				ExpirationMinutes = 60
			};

			services.AddFinanceManagerInfrastructure(
				options => options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}"),
				jwtSettings);
		});
	}
}
