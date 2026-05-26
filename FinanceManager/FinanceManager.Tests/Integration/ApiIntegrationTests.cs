using System.Net.Http.Json;
using FinanceManager.Application.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FinanceManager.Tests.Integration;

public class ApiIntegrationTests : IClassFixture<FinanceManagerWebApplicationFactory>
{
	private readonly FinanceManagerWebApplicationFactory factory;

	public ApiIntegrationTests(FinanceManagerWebApplicationFactory factory)
	{
		this.factory = factory;
	}

	[Fact]
	public async Task RegisterAndLogin_ReturnTokens()
	{
		var client = factory.CreateClient();
		var username = $"user_{Guid.NewGuid():N}_{Random.Shared.Next(10000, 99999)}";
		var registerResponse = await client.PostAsJsonAsync("/api/auth/register", new { username, password = "Password123!" });
		if (!registerResponse.IsSuccessStatusCode)
		{
			var body = await registerResponse.Content.ReadAsStringAsync();
			Assert.Fail($"Register failed: {registerResponse.StatusCode} - {body}");
		}

		var tokens = await registerResponse.Content.ReadFromJsonAsync<AuthTokensDto>();
		Assert.NotNull(tokens);
		Assert.False(string.IsNullOrEmpty(tokens.AccessToken));
		Assert.False(string.IsNullOrEmpty(tokens.RefreshToken));
	}

	[Fact]
	public async Task ProtectedEndpoint_WithoutToken_ReturnsUnauthorized()
	{
		var client = factory.CreateClient();
		var response = await client.GetAsync("/api/expenses");
		Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
	}
}
