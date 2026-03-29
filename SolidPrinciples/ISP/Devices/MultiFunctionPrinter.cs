using ISP.Interfaces;

namespace ISP.Devices;
public class MultiFunctionPrinter : IMultiFunctionDevice
{
    public void Print(string document)
    {
        Console.WriteLine($"[MultiFunctionPrinter] Printing: {document}");
    }

    public void Scan(string document)
    {
        Console.WriteLine($"[MultiFunctionPrinter] Scanning: {document}");
    }

    public void Fax(string document)
    {
        Console.WriteLine($"[MultiFunctionPrinter] Faxing: {document}");
    }
}
