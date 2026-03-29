using ISP.Interfaces;

namespace ISP.Devices;
public class SimpleScanner : IScanner
{
    public void Scan(string document)
    {
        Console.WriteLine($"[SimpleScanner] Scanning: {document}");
    }
}
