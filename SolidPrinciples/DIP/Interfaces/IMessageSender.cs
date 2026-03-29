namespace DIP.Interfaces;
public interface IMessageSender
{
    void Send(string recipient, string message);
}
