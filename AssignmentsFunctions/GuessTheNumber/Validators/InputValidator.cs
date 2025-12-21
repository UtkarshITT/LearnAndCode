namespace GuessTheNumber.Validators;

public static class InputValidator
{
    private const int MinValue = 1;
    private const int MaxValue = 100;

    public static bool IsValidGuess(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        if (!int.TryParse(input, out int number))
            return false;

        return number >= MinValue && number <= MaxValue;
    }
    public static int ParseGuess(string input)
    {
        return int.Parse(input);
    }
}
