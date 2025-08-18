using ConstructionEstimator.Core.Models;

namespace ConstructionEstimator.Core.Services;

public interface INotificationService
{
    Task SendPriceChangeNotificationAsync(MaterialPrice oldPrice, MaterialPrice newPrice);
    Task SendSystemNotificationAsync(string message, string category = "System");
    event EventHandler<NotificationEventArgs>? NotificationReceived;
}

public class NotificationEventArgs : EventArgs
{
    public string Message { get; }
    public string Category { get; }
    public DateTime Timestamp { get; }

    public NotificationEventArgs(string message, string category = "General")
    {
        Message = message;
        Category = category;
        Timestamp = DateTime.Now;
    }
}