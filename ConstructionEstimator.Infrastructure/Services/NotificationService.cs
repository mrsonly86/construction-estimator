using ConstructionEstimator.Core.Models;
using ConstructionEstimator.Core.Services;

namespace ConstructionEstimator.Infrastructure.Services;

public class NotificationService : INotificationService
{
    public event EventHandler<NotificationEventArgs>? NotificationReceived;

    public async Task SendPriceChangeNotificationAsync(MaterialPrice oldPrice, MaterialPrice newPrice)
    {
        await Task.Delay(100); // Simulate async operation
        
        var changePercent = oldPrice.Price != 0 
            ? Math.Round(((newPrice.Price - oldPrice.Price) / oldPrice.Price) * 100, 2)
            : 0;
        
        var direction = newPrice.Price > oldPrice.Price ? "tăng" : "giảm";
        var message = $"Giá {newPrice.MaterialName} tại {newPrice.Province} {direction} " +
                     $"{Math.Abs(changePercent):F2}% từ {oldPrice.Price:N0} VND/{oldPrice.Unit} " +
                     $"lên {newPrice.Price:N0} VND/{newPrice.Unit}";
        
        OnNotificationReceived(new NotificationEventArgs(message, "PriceChange"));
        
        // Log to console for demo purposes
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] PRICE CHANGE: {message}");
    }

    public async Task SendSystemNotificationAsync(string message, string category = "System")
    {
        await Task.Delay(50); // Simulate async operation
        
        OnNotificationReceived(new NotificationEventArgs(message, category));
        
        // Log to console for demo purposes
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {category.ToUpper()}: {message}");
    }

    protected virtual void OnNotificationReceived(NotificationEventArgs e)
    {
        NotificationReceived?.Invoke(this, e);
    }
}