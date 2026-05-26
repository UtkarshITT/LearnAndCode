using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FinanceManager.Application.DTOs;
using FinanceManager.Application.Enums;
using FinanceManager.ConsoleUI.Session;

namespace FinanceManager.ConsoleUI.Api;

public class FinanceApiClient
{
	private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

	private readonly HttpClient httpClient;
	private readonly UserSession session;

	public FinanceApiClient(HttpClient httpClient, UserSession session)
	{
		this.httpClient = httpClient;
		this.session = session;
	}

	public async Task<AuthTokensDto> RegisterAsync(string username, string password)
	{
		var response = await httpClient.PostAsJsonAsync(
			"/api/auth/register",
			new LoginRequest { Username = username, Password = password });
		return await ReadAuthResponseAsync(response);
	}

	public async Task<AuthTokensDto> LoginAsync(string username, string password)
	{
		var response = await httpClient.PostAsJsonAsync(
			"/api/auth/login",
			new LoginRequest { Username = username, Password = password });
		return await ReadAuthResponseAsync(response);
	}

	public async Task LogoutAsync()
	{
		if (!string.IsNullOrEmpty(session.RefreshToken))
		{
			await httpClient.PostAsJsonAsync(
				"/api/auth/logout",
				new RefreshRequest { RefreshToken = session.RefreshToken });
		}

		session.Clear();
	}

	public async Task<IReadOnlyList<TransactionDto>> GetExpensesAsync(
		string? keyword,
		ExpenseSortField sortBy,
		SortDirection sortDirection)
	{
		var url = $"/api/expenses?sortBy={sortBy}&sortDirection={sortDirection}";
		if (!string.IsNullOrWhiteSpace(keyword))
		{
			url += $"&keyword={Uri.EscapeDataString(keyword)}";
		}

		return await GetAsync<IReadOnlyList<TransactionDto>>(url) ?? Array.Empty<TransactionDto>();
	}

	public async Task<TransactionDto?> CreateExpenseAsync(CreateTransactionRequest request)
	{
		return await PostAsync<TransactionDto>("/api/expenses", request);
	}

	public async Task<TransactionDto?> UpdateExpenseAsync(int id, UpdateTransactionRequest request)
	{
		return await PutAsync<TransactionDto>($"/api/expenses/{id}", request);
	}

	public Task<bool> DeleteExpenseAsync(int id) => DeleteAsync($"/api/expenses/{id}");

	public async Task<IReadOnlyList<TransactionDto>> GetIncomeAsync()
	{
		return await GetAsync<IReadOnlyList<TransactionDto>>("/api/income") ?? Array.Empty<TransactionDto>();
	}

	public async Task<TransactionDto?> CreateIncomeAsync(CreateTransactionRequest request)
	{
		return await PostAsync<TransactionDto>("/api/income", request);
	}

	public async Task<TransactionDto?> UpdateIncomeAsync(int id, UpdateTransactionRequest request)
	{
		return await PutAsync<TransactionDto>($"/api/income/{id}", request);
	}

	public Task<bool> DeleteIncomeAsync(int id) => DeleteAsync($"/api/income/{id}");

	public async Task<IReadOnlyList<BudgetDto>> GetBudgetsAsync()
	{
		return await GetAsync<IReadOnlyList<BudgetDto>>("/api/budgets") ?? Array.Empty<BudgetDto>();
	}

	public async Task<BudgetDto?> CreateBudgetAsync(CreateBudgetRequest request)
	{
		return await PostAsync<BudgetDto>("/api/budgets", request);
	}

	public async Task<BudgetDto?> UpdateBudgetAsync(int id, UpdateBudgetRequest request)
	{
		return await PutAsync<BudgetDto>($"/api/budgets/{id}", request);
	}

	public Task<bool> DeleteBudgetAsync(int id) => DeleteAsync($"/api/budgets/{id}");

	public async Task<SummaryDto?> GetSummaryAsync()
	{
		return await GetAsync<SummaryDto>("/api/summary");
	}

	public async Task<IReadOnlyList<CategorySummaryDto>> GetSummaryByCategoryAsync()
	{
		return await GetAsync<IReadOnlyList<CategorySummaryDto>>("/api/summary/by-category")
			?? Array.Empty<CategorySummaryDto>();
	}

	private async Task<AuthTokensDto> ReadAuthResponseAsync(HttpResponseMessage response)
	{
		if (!response.IsSuccessStatusCode)
		{
			var error = await response.Content.ReadAsStringAsync();
			throw new HttpRequestException(ParseErrorMessage(error, response.StatusCode));
		}

		var tokens = await response.Content.ReadFromJsonAsync<AuthTokensDto>(JsonOptions);
		if (tokens == null)
		{
			throw new InvalidOperationException("Empty auth response.");
		}

		session.SetTokens(tokens.AccessToken, tokens.RefreshToken);
		return tokens;
	}

	private async Task<T?> GetAsync<T>(string url)
	{
		var response = await SendAuthorizedAsync(client => client.GetAsync(url));
		return await ReadSuccessAsync<T>(response);
	}

	private async Task<T?> PostAsync<T>(string url, object body)
	{
		var response = await SendAuthorizedAsync(client => client.PostAsJsonAsync(url, body));
		return await ReadSuccessAsync<T>(response);
	}

	private async Task<T?> PutAsync<T>(string url, object body)
	{
		var response = await SendAuthorizedAsync(client => client.PutAsJsonAsync(url, body));
		return await ReadSuccessAsync<T>(response);
	}

	private async Task<bool> DeleteAsync(string url)
	{
		var response = await SendAuthorizedAsync(client => client.DeleteAsync(url));
		return response.IsSuccessStatusCode;
	}

	private async Task<HttpResponseMessage> SendAuthorizedAsync(
		Func<HttpClient, Task<HttpResponseMessage>> sendRequest)
	{
		await EnsureAuthHeaderAsync();
		var response = await sendRequest(httpClient);
		if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
		{
			await TryRefreshAsync();
			response = await sendRequest(httpClient);
		}

		return response;
	}

	private async Task<T?> ReadSuccessAsync<T>(HttpResponseMessage response)
	{
		if (!response.IsSuccessStatusCode)
		{
			throw new HttpRequestException(await response.Content.ReadAsStringAsync());
		}

		return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
	}

	private Task EnsureAuthHeaderAsync()
	{
		if (string.IsNullOrEmpty(session.AccessToken))
		{
			throw new UnauthorizedAccessException("Not logged in.");
		}

		httpClient.DefaultRequestHeaders.Authorization =
			new AuthenticationHeaderValue("Bearer", session.AccessToken);
		return Task.CompletedTask;
	}

	private async Task TryRefreshAsync()
	{
		if (string.IsNullOrEmpty(session.RefreshToken))
		{
			throw new UnauthorizedAccessException("Session expired. Please login again.");
		}

		var response = await httpClient.PostAsJsonAsync(
			"/api/auth/refresh",
			new RefreshRequest { RefreshToken = session.RefreshToken });

		if (!response.IsSuccessStatusCode)
		{
			session.Clear();
			throw new UnauthorizedAccessException("Session expired. Please login again.");
		}

		var tokens = await response.Content.ReadFromJsonAsync<AuthTokensDto>(JsonOptions);
		if (tokens != null)
		{
			session.SetTokens(tokens.AccessToken, tokens.RefreshToken);
		}
	}

	private static string ParseErrorMessage(string body, System.Net.HttpStatusCode statusCode)
	{
		try
		{
			using var doc = JsonDocument.Parse(body);
			if (doc.RootElement.TryGetProperty("message", out var message))
			{
				return message.GetString() ?? statusCode.ToString();
			}
		}
		catch
		{
			// ignore parse errors
		}

		return $"{statusCode}: {body}";
	}
}
