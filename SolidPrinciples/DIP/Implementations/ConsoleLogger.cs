using DIP.Interfaces;

namespace DIP.Implementations;
public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[Console Log] {DateTime.Now:HH:mm:ss}: {message}");
    }
}
