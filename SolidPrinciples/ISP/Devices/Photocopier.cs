using ISP.Interfaces;

namespace ISP.Devices;
public class Photocopier : IPrinter, IScanner
{
    public void Print(string document)
    {
        Console.WriteLine($"[Photocopier] Printing: {document}");
    }

    public void Scan(string document)
    {
        Console.WriteLine($"[Photocopier] Scanning: {document}");
    }

    public void Copy(string document)
    {
        Console.WriteLine($"[Photocopier] Copying: {document}");
        Scan(document);
        Print(document);
    }
}
