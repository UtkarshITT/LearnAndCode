using DIP.Interfaces;

namespace DIP.Implementations;
public class EmailSender : IMessageSender
{
    public void Send(string recipient, string message)
    {
        Console.WriteLine($"[Email] Sending to {recipient}: {message}");
    }
}
