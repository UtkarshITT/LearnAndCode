namespace GuessTheNumber.Services;
public class NumberGenerator
{
    private readonly Random _random;
    private const int MinValue = 1;
    private const int MaxValue = 100;

    public NumberGenerator()
    {
        _random = new Random();
    }
    public int GenerateSecretNumber()
    {
        return _random.Next(MinValue, MaxValue + 1);
    }
}
