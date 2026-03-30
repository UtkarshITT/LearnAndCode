using BookSRP.Interfaces;
using BookSRP.Models;

namespace BookSRP.Services;

/// <summary>
/// Library locator - Single Responsibility: Find book locations in the library
/// </summary>
public class LibraryLocator : ILibraryLocator
{
    private readonly Dictionary<string, LibraryLocation> _locationMap;

    public LibraryLocator()
    {
        // Simulated location data
        _locationMap = new Dictionary<string, LibraryLocation>
        {
            { "A Great Book", new LibraryLocation("Room A", "Shelf 3") },
            { "Another Book", new LibraryLocation("Room B", "Shelf 7") }
        };
    }

    public LibraryLocation GetLocation(Book book)
    {
        if (_locationMap.TryGetValue(book.Title, out var location))
        {
            return location;
        }

        return new LibraryLocation("Unknown", "Unknown");
    }
}
