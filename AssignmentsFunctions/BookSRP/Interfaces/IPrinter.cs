namespace BookSRP.Interfaces;

/// <summary>
/// Interface for printing pages - Single Responsibility: Define print contract
/// </summary>
public interface IPrinter
{
    void PrintPage(string page);
}
