using DIP.Interfaces;

namespace DIP.Services;
public class NotificationService
{
    private readonly IMessageSender _messageSender;
    private readonly IDataRepository _repository;
    private readonly ILogger _logger;

    public NotificationService(
        IMessageSender messageSender,
        IDataRepository repository,
        ILogger logger)
    {
        _messageSender = messageSender;
        _repository = repository;
        _logger = logger;
    }

    public void SendNotification(string recipient, string message)
    {
        _logger.Log($"Preparing notification for {recipient}");
        
        _messageSender.Send(recipient, message);
        
        _repository.Save($"Notification sent to {recipient}: {message}");
        
        _logger.Log($"Notification completed for {recipient}");
    }

    public void SendBulkNotification(IEnumerable<string> recipients, string message)
    {
        _logger.Log($"Starting bulk notification to {recipients.Count()} recipients");
        
        foreach (var recipient in recipients)
        {
            _messageSender.Send(recipient, message);
        }
        
        _repository.Save($"Bulk notification sent to {recipients.Count()} recipients");
        _logger.Log("Bulk notification completed");
    }
}
