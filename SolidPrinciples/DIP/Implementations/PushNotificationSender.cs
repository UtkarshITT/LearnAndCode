using DIP.Interfaces;

namespace DIP.Implementations;
public class PushNotificationSender : IMessageSender
{
    public void Send(string recipient, string message)
    {
        Console.WriteLine($"[Push] Sending to {recipient}: {message}");
    }
}
