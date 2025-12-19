using DIP.Interfaces;

namespace DIP.Implementations;

public class FileLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[File Log] {DateTime.Now:HH:mm:ss}: {message} -> Written to log.txt");
    }
}
