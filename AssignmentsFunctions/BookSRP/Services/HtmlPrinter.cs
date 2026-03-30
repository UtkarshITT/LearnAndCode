using BookSRP.Interfaces;

namespace BookSRP.Services;

/// <summary>
/// HTML printer - Single Responsibility: Print pages as HTML format
/// </summary>
public class HtmlPrinter : IPrinter
{
    public void PrintPage(string page)
    {
        Console.WriteLine($"<div class=\"single-page\">{page}</div>");
    }
}
