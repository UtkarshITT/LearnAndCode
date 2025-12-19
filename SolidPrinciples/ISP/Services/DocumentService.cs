using ISP.Interfaces;

namespace ISP.Services;

public class DocumentService
{
    public void PrintDocument(IPrinter printer, string document)
    {
        Console.WriteLine("Document Service - Print Request:");
        printer.Print(document);
    }

    public void ScanDocument(IScanner scanner, string document)
    {
        Console.WriteLine("Document Service - Scan Request:");
        scanner.Scan(document);
    }

    public void FaxDocument(IFax fax, string document)
    {
        Console.WriteLine("Document Service - Fax Request:");
        fax.Fax(document);
    }
}
