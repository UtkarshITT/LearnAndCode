using BookSRP.Interfaces;

namespace BookSRP.Services;

/// <summary>
/// Plain text printer - Single Responsibility: Print pages as plain text
/// </summary>
public class PlainTextPrinter : IPrinter
{
    public void PrintPage(string page)
    {
        Console.WriteLine(page);
    }
}
