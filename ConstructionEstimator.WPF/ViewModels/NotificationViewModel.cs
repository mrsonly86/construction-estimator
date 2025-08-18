using System.Collections.ObjectModel;
using ConstructionEstimator.Core.Services;

namespace ConstructionEstimator.WPF.ViewModels;

public class NotificationViewModel : ViewModelBase
{
    private readonly INotificationService _notificationService;

    public NotificationViewModel(INotificationService notificationService)
    {
        _notificationService = notificationService;
        Notifications = new ObservableCollection<NotificationItem>();
        
        _notificationService.NotificationReceived += OnNotificationReceived;
    }

    public ObservableCollection<NotificationItem> Notifications { get; }

    private void OnNotificationReceived(object? sender, NotificationEventArgs e)
    {
        var notification = new NotificationItem
        {
            Message = e.Message,
            Category = e.Category,
            Timestamp = e.Timestamp
        };

        // Add to the beginning of the collection for latest-first display
        Notifications.Insert(0, notification);

        // Keep only last 100 notifications
        while (Notifications.Count > 100)
        {
            Notifications.RemoveAt(Notifications.Count - 1);
        }
    }

    public void ClearNotifications()
    {
        Notifications.Clear();
    }
}

public class NotificationItem
{
    public string Message { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    
    public string DisplayText => $"[{Timestamp:HH:mm:ss}] {Category}: {Message}";
}