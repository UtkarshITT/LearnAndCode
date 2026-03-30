using BookSRP.Models;

namespace BookSRP.Interfaces;

/// <summary>
/// Interface for book persistence - Single Responsibility: Define save/load contract
/// </summary>
public interface IBookRepository
{
    void Save(Book book);
    Book? Load(string title, string author);
}
