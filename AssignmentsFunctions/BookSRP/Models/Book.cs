namespace BookSRP.Models;

/// <summary>
/// Book entity - Single Responsibility: Hold book information
/// Does NOT handle saving, location, or printing
/// </summary>
public class Book
{
    public string Title { get; }
    public string Author { get; }
    public List<string> Pages { get; }
    public int CurrentPageIndex { get; private set; }

    public Book(string title, string author, List<string> pages)
    {
        Title = title;
        Author = author;
        Pages = pages;
        CurrentPageIndex = 0;
    }

    public string GetCurrentPage()
    {
        if (Pages.Count == 0)
            return string.Empty;

        return Pages[CurrentPageIndex];
    }

    public void TurnPage()
    {
        if (CurrentPageIndex < Pages.Count - 1)
        {
            CurrentPageIndex++;
        }
    }

    public void TurnBackPage()
    {
        if (CurrentPageIndex > 0)
        {
            CurrentPageIndex--;
        }
    }
}
