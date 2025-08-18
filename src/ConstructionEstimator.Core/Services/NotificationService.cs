using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConstructionEstimator.Core.Services;

public class NotificationService : INotificationService
{
    private readonly IPriceAlertRepository _priceAlertRepository;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(IPriceAlertRepository priceAlertRepository, ILogger<NotificationService> logger)
    {
        _priceAlertRepository = priceAlertRepository;
        _logger = logger;
    }

    public async Task SendPriceAlertAsync(PriceAlert alert, PriceHistory priceChange)
    {
        try
        {
            _logger.LogInformation("Sending price alert for material {MaterialId}: {ChangeType} of {ChangePercentage:F2}%", 
                alert.MaterialId, priceChange.ChangeType, priceChange.ChangePercentage);

            // Update alert trigger information
            alert.LastTriggered = DateTime.Now;
            alert.TriggerCount++;
            await _priceAlertRepository.UpdateAsync(alert);
            await _priceAlertRepository.SaveChangesAsync();

            // Here you would implement actual notification sending
            // For now, we'll log the alert details
            var materialName = priceChange.MaterialPrice.Material.Name;
            var provinceName = priceChange.MaterialPrice.Province.Name;
            var oldPrice = priceChange.OldPrice;
            var newPrice = priceChange.NewPrice;

            var message = $"Cảnh báo giá vật liệu: {materialName} tại {provinceName}\n" +
                         $"Giá cũ: {oldPrice:N0} VND\n" +
                         $"Giá mới: {newPrice:N0} VND\n" +
                         $"Thay đổi: {priceChange.ChangeType} {Math.Abs(priceChange.ChangePercentage):F2}%\n" +
                         $"Ngày: {priceChange.ChangeDate:dd/MM/yyyy HH:mm}";

            _logger.LogWarning("PRICE ALERT: {Message}", message);

            // TODO: Implement actual notification mechanisms
            if (alert.EmailEnabled && !string.IsNullOrEmpty(alert.NotificationEmail))
            {
                await SendEmailNotificationAsync(alert.NotificationEmail, materialName, message);
            }

            if (alert.PopupEnabled)
            {
                await SendPopupNotificationAsync(materialName, message);
            }

            _logger.LogInformation("Price alert sent successfully for material {MaterialId}", alert.MaterialId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending price alert for material {MaterialId}", alert.MaterialId);
            throw;
        }
    }

    public async Task<IEnumerable<PriceAlert>> GetActiveAlertsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all active price alerts");
            return await _priceAlertRepository.GetActiveAlertsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active price alerts");
            throw;
        }
    }

    public async Task<PriceAlert> CreatePriceAlertAsync(PriceAlert alert)
    {
        try
        {
            _logger.LogInformation("Creating new price alert for material {MaterialId}", alert.MaterialId);
            
            alert.CreatedDate = DateTime.Now;
            alert.IsActive = true;
            alert.TriggerCount = 0;

            var createdAlert = await _priceAlertRepository.AddAsync(alert);
            await _priceAlertRepository.SaveChangesAsync();

            _logger.LogInformation("Successfully created price alert with ID: {AlertId}", createdAlert.Id);
            return createdAlert;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating price alert for material {MaterialId}", alert.MaterialId);
            throw;
        }
    }

    public async Task<PriceAlert> UpdatePriceAlertAsync(PriceAlert alert)
    {
        try
        {
            _logger.LogInformation("Updating price alert {AlertId} for material {MaterialId}", 
                alert.Id, alert.MaterialId);
            
            await _priceAlertRepository.UpdateAsync(alert);
            await _priceAlertRepository.SaveChangesAsync();

            _logger.LogInformation("Successfully updated price alert {AlertId}", alert.Id);
            return alert;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating price alert {AlertId}", alert.Id);
            throw;
        }
    }

    public async Task DeletePriceAlertAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting price alert {AlertId}", id);
            await _priceAlertRepository.DeleteAsync(id);
            await _priceAlertRepository.SaveChangesAsync();
            _logger.LogInformation("Successfully deleted price alert {AlertId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting price alert {AlertId}", id);
            throw;
        }
    }

    public async Task CheckAndTriggerAlertsAsync(int materialId, int provinceId, decimal priceChange)
    {
        try
        {
            _logger.LogInformation("Checking alerts for material {MaterialId} in province {ProvinceId} with price change {PriceChange:F2}%", 
                materialId, provinceId, priceChange);

            var alerts = await _priceAlertRepository.GetAlertsForMaterialAsync(materialId);
            var provinceAlerts = alerts.Where(a => a.ProvinceId == null || a.ProvinceId == provinceId);

            foreach (var alert in provinceAlerts)
            {
                var shouldTrigger = false;
                var absChange = Math.Abs(priceChange);

                switch (alert.AlertType.ToLower())
                {
                    case "priceincrease":
                        shouldTrigger = priceChange > 0 && absChange >= alert.ThresholdPercentage;
                        break;
                    case "pricedecrease":
                        shouldTrigger = priceChange < 0 && absChange >= alert.ThresholdPercentage;
                        break;
                    case "significantchange":
                        shouldTrigger = absChange >= alert.ThresholdPercentage;
                        break;
                }

                if (shouldTrigger)
                {
                    _logger.LogInformation("Triggering alert {AlertId} for material {MaterialId}", 
                        alert.Id, materialId);
                    
                    // Get the price history record for this change
                    // For now, we'll create a mock PriceHistory object
                    // In a real implementation, this would be passed or retrieved
                    var mockPriceHistory = new PriceHistory
                    {
                        ChangePercentage = priceChange,
                        ChangeType = priceChange > 0 ? "Increase" : "Decrease",
                        ChangeDate = DateTime.Now,
                        MaterialPrice = new MaterialPrice
                        {
                            Material = new Material { Name = $"Material {materialId}" },
                            Province = new Province { Name = $"Province {provinceId}" }
                        }
                    };

                    // Note: In production, this should be queued for background processing
                    // to avoid blocking the price update process
                    _ = Task.Run(() => SendPriceAlertAsync(alert, mockPriceHistory));
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking alerts for material {MaterialId} in province {ProvinceId}", 
                materialId, provinceId);
            throw;
        }
    }

    private async Task SendEmailNotificationAsync(string email, string materialName, string message)
    {
        // TODO: Implement email notification
        // This could use services like SendGrid, SMTP, etc.
        _logger.LogInformation("Email notification would be sent to {Email} for material {MaterialName}", 
            email, materialName);
        await Task.CompletedTask;
    }

    private async Task SendPopupNotificationAsync(string materialName, string message)
    {
        // TODO: Implement popup notification for WPF application
        // This could use a notification service that the UI subscribes to
        _logger.LogInformation("Popup notification would be displayed for material {MaterialName}", materialName);
        await Task.CompletedTask;
    }
}