using BookSRP.Models;

namespace BookSRP.Interfaces;

/// <summary>
/// Interface for locating books - Single Responsibility: Define location contract
/// </summary>
public interface ILibraryLocator
{
    LibraryLocation GetLocation(Book book);
}
