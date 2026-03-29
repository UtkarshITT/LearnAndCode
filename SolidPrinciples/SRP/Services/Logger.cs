namespace SRP.Services;
public class Logger
{
    public void Log(string message)
    {
        Console.WriteLine($"[Log] {DateTime.Now}: {message}");
    }
}
