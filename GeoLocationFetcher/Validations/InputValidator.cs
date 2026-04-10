namespace GeoLocationFetcher.Validations;

public static class InputValidator
{
	public static bool isValid(string input)
	{
		if (string.IsNullOrWhiteSpace(input))
		{
			return false;
		}

		var trimmed = input.Trim();

		if (trimmed.Length < 2)
		{
			return false;
		}

		if (trimmed.All(char.IsDigit))
		{
			return false;
		}

		return true;
	}
}
