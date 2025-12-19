using DIP.Interfaces;

namespace DIP.Implementations;
public class SmsSender : IMessageSender
{
    public void Send(string recipient, string message)
    {
        Console.WriteLine($"[SMS] Sending to {recipient}: {message}");
    }
}
