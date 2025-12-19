using ISP.Devices;
using ISP.Services;

Console.WriteLine("INTERFACE SEGREGATION PRINCIPLE (ISP) EXAMPLE");
Console.WriteLine("==============================================");
Console.WriteLine();
Console.WriteLine("ISP: Clients should not be forced to depend on interfaces they don't use.");
Console.WriteLine("Split large interfaces into smaller, focused ones.");
Console.WriteLine();

var documentService = new DocumentService();

// Simple Printer - only implements IPrinter
Console.WriteLine("1. Simple Printer (only prints):");
Console.WriteLine("--------------------------------");
var simplePrinter = new SimplePrinter();
documentService.PrintDocument(simplePrinter, "Report.pdf");
Console.WriteLine();

// Simple Scanner - only implements IScanner
Console.WriteLine("2. Simple Scanner (only scans):");
Console.WriteLine("-------------------------------");
var simpleScanner = new SimpleScanner();
documentService.ScanDocument(simpleScanner, "Photo.jpg");
Console.WriteLine();

// Photocopier - implements IPrinter and IScanner
Console.WriteLine("3. Photocopier (prints and scans):");
Console.WriteLine("-----------------------------------");
var photocopier = new Photocopier();
documentService.PrintDocument(photocopier, "Contract.pdf");
documentService.ScanDocument(photocopier, "Invoice.pdf");
photocopier.Copy("Document.pdf");
Console.WriteLine();

// Multi-function Printer - implements all interfaces
Console.WriteLine("4. Multi-Function Printer (prints, scans, and faxes):");
Console.WriteLine("------------------------------------------------------");
var mfp = new MultiFunctionPrinter();
documentService.PrintDocument(mfp, "Letter.docx");
documentService.ScanDocument(mfp, "ID Card");
documentService.FaxDocument(mfp, "Agreement.pdf");
Console.WriteLine();

Console.WriteLine("Benefits:");
Console.WriteLine("  - Devices only implement interfaces they actually support");
Console.WriteLine("  - No empty or throwing methods for unsupported features");
Console.WriteLine("  - Clients depend only on the interfaces they need");
Console.WriteLine("  - Easy to add new device types with specific capabilities");
