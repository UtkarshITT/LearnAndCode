using ISP.Interfaces;

namespace ISP.Devices;
public class SimplePrinter : IPrinter
{
    public void Print(string document)
    {
        Console.WriteLine($"[SimplePrinter] Printing: {document}");
    }
}
