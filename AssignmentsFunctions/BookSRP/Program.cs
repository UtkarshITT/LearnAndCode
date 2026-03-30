using BookSRP.Interfaces;
using BookSRP.Models;
using BookSRP.Repositories;
using BookSRP.Services;

/*
 * Book SRP Example
 * 
 * Original Book class violated SRP by having multiple responsibilities:
 *   - Book information (title, author)
 *   - Page navigation (turnPage, getCurrentPage)
 *   - Library location (getLocation) 
 *   - Persistence (save)
 *   - Printing was mixed in
 * 
 * Refactored to follow SRP:
 *   - Book: Only holds book data and page navigation
 *   - BookRepository: Handles persistence (save/load)
 *   - LibraryLocator: Handles finding book location
 *   - IPrinter implementations: Handle printing
 */

Console.WriteLine("BOOK SRP EXAMPLE");
Console.WriteLine("================");
Console.WriteLine();
Console.WriteLine("Each class now has a SINGLE responsibility:");
Console.WriteLine("  - Book: Hold book data and page navigation");
Console.WriteLine("  - BookRepository: Save/load books");
Console.WriteLine("  - LibraryLocator: Find book locations");
Console.WriteLine("  - PlainTextPrinter/HtmlPrinter: Print pages");
Console.WriteLine();

// Create a book with some pages
var book = new Book(
    "A Great Book",
    "John Doe",
    new List<string>
    {
        "Chapter 1: The Beginning - Once upon a time...",
        "Chapter 2: The Journey - They traveled far...",
        "Chapter 3: The End - And they lived happily ever after."
    }
);

Console.WriteLine($"Book: {book.Title} by {book.Author}");
Console.WriteLine();

// Book handles page navigation (its responsibility)
Console.WriteLine("1. Page Navigation (Book's responsibility):");
Console.WriteLine("-------------------------------------------");
Console.WriteLine($"Current page: {book.GetCurrentPage()}");
book.TurnPage();
Console.WriteLine($"After turning: {book.GetCurrentPage()}");
Console.WriteLine();

// Repository handles persistence (separate responsibility)
Console.WriteLine("2. Persistence (BookRepository's responsibility):");
Console.WriteLine("-------------------------------------------------");
IBookRepository repository = new BookRepository();
repository.Save(book);
Console.WriteLine();

// Locator handles library location (separate responsibility)
Console.WriteLine("3. Location (LibraryLocator's responsibility):");
Console.WriteLine("-----------------------------------------------");
ILibraryLocator locator = new LibraryLocator();
var location = locator.GetLocation(book);
Console.WriteLine($"Book location: {location}");
Console.WriteLine();

// Printers handle printing (separate responsibility)
Console.WriteLine("4. Printing (Printer's responsibility):");
Console.WriteLine("----------------------------------------");
Console.WriteLine("Plain Text:");
IPrinter plainPrinter = new PlainTextPrinter();
plainPrinter.PrintPage(book.GetCurrentPage());
Console.WriteLine();

Console.WriteLine("HTML Format:");
IPrinter htmlPrinter = new HtmlPrinter();
htmlPrinter.PrintPage(book.GetCurrentPage());
Console.WriteLine();

Console.WriteLine("Benefits of SRP:");
Console.WriteLine("  ✓ Book class is simpler and focused");
Console.WriteLine("  ✓ Can change save logic without touching Book");
Console.WriteLine("  ✓ Can add new printer types easily");
Console.WriteLine("  ✓ Each class is testable independently");
