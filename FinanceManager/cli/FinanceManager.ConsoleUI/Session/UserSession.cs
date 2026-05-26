namespace FinanceManager.ConsoleUI.Session;

public class UserSession
{
	public string? AccessToken { get; private set; }

	public string? RefreshToken { get; private set; }

	public int? LastCategoryId { get; set; }

	public bool UseDefaults { get; set; }

	public bool IsAuthenticated => !string.IsNullOrEmpty(AccessToken);

	public void SetTokens(string accessToken, string refreshToken)
	{
		AccessToken = accessToken;
		RefreshToken = refreshToken;
	}

	public void Clear()
	{
		AccessToken = null;
		RefreshToken = null;
		LastCategoryId = null;
	}
}
