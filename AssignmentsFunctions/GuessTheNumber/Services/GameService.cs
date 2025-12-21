using GuessTheNumber.Validators;

namespace GuessTheNumber.Services;
public class GameService
{
    private readonly NumberGenerator _numberGenerator;
    private int _secretNumber;
    private int _guessCount;
    private bool _hasGuessedCorrectly;

    public GameService(NumberGenerator numberGenerator)
    {
        _numberGenerator = numberGenerator;
        _guessCount = 0;
        _hasGuessedCorrectly = false;
    }

    public void Play()
    {
        _secretNumber = _numberGenerator.GenerateSecretNumber();
        _guessCount = 0;
        _hasGuessedCorrectly = false;

        Console.WriteLine("Welcome to Guess The Number!");
        Console.WriteLine("I'm thinking of a number between 1 and 100.");
        Console.WriteLine();

        while (!_hasGuessedCorrectly)
        {
            Console.Write("Guess a number between 1 and 100: ");
            string? input = Console.ReadLine();

            ProcessGuess(input);
        }
    }

    private void ProcessGuess(string? input)
    {
        if (!InputValidator.IsValidGuess(input))
        {
            Console.WriteLine("I won't count this one. Please enter a number between 1 to 100.");
            Console.WriteLine();
            return;
        }

        int guess = InputValidator.ParseGuess(input!);
        _guessCount++;

        if (guess < _secretNumber)
        {
            Console.WriteLine("Too low. Guess again.");
            Console.WriteLine();
        }
        else if (guess > _secretNumber)
        {
            Console.WriteLine("Too high. Guess again.");
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine($"Congratulations! You guessed it in {_guessCount} guesses!");
            _hasGuessedCorrectly = true;
        }
    }
}
